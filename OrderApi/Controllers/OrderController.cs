using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderApi.Dto;
using OrderApi.Dtos;
using OrderApi.Model;
using OrderApi.MongoDbConfigurations;


namespace OrderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderController(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _orders = database.GetCollection<Order>("Orders");
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetAll()
        {
            var orders = await _orders.Find(_ => true).ToListAsync();
            var result = orders.Select(MapToDto).ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(string id)
        {
            var order = await _orders.Find(x => x.OrderId == id).FirstOrDefaultAsync();
            if (order == null)
                return NotFound();
            return Ok(MapToDto(order));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] OrderDto orderDto)
        {
            var order = MapToModel(orderDto);
            await _orders.InsertOneAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, MapToDto(order));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] OrderDto orderDto)
        {
            var existing = await _orders.Find(x => x.OrderId == id).FirstOrDefaultAsync();
            if (existing == null)
                return NotFound();

            var updatedOrder = MapToModel(orderDto);
            updatedOrder.OrderId = id;

            await _orders.ReplaceOneAsync(x => x.OrderId == id, updatedOrder);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _orders.DeleteOneAsync(x => x.OrderId == id);
            if (result.DeletedCount == 0)
                return NotFound();
            return NoContent();
        }

        private OrderDto MapToDto(Order order) => new OrderDto
        {
            OrderId = order.OrderId,
            CustomerId = order.CustomerId,
            OrderedOn = order.OrderedOn,
            OrderDetails = order.OrderDetails?.Select(od => new OrderDetailDto
            {
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                Price = od.Price,
                UnitPrice = od.UnitPrice
            }).ToList()
        };

        private Order MapToModel(OrderDto dto) => new Order
        {
            CustomerId = dto.CustomerId,
            OrderedOn = dto.OrderedOn,
            OrderDetails = dto.OrderDetails?.Select(od => new OrderDetail
            {
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                Price = od.Price,
                UnitPrice = od.UnitPrice
            }).ToList()
        };
    }
}
