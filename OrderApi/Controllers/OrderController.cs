using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderApi.Dto;
using OrderApi.Dtos;
using OrderApi.Interface;
using OrderApi.Model;
using OrderApi.MongoDbConfigurations;


namespace OrderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetAll()
        {
            var result = await _orderService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(string id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] OrderDto orderDto,CancellationToken cancellationToken)
        {
            var created = await _orderService.CreateAsync(orderDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.OrderId }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] OrderDto orderDto)
        {
            var success = await _orderService.UpdateAsync(id, orderDto);
            if (!success)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var success = await _orderService.DeleteAsync(id);
            if (!success)
                return NotFound();
            return NoContent();
        }
    }
}
