using System.Threading;
using Messaging.Interfaces;
using Messaging.Models;
using Messaging.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderApi.Dto;
using OrderApi.Dtos;
using OrderApi.Interface;
using OrderApi.Model;
using OrderApi.MongoDbConfigurations;
using SharedContracts;

namespace OrderApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMongoCollection<Order> _orders;
    

        public OrderService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _orders = database.GetCollection<Order>("Orders");
        }

        public async Task<List<OrderDto>> GetAllAsync()
        {
            var orders = await _orders.Find(_ => true).ToListAsync();
            return orders.Select(MapToDto).ToList();
        }

        public async Task<OrderDto> GetByIdAsync(string id)
        {
            var order = await _orders.Find(x => x.OrderId == id).FirstOrDefaultAsync();
            return order == null ? null : MapToDto(order);
        }

        public async Task<OrderDto> CreateAsync(OrderDto orderDto, CancellationToken cancellationToken)
        {
            var order = MapToModel(orderDto);

            await _orders.InsertOneAsync(order, cancellationToken: cancellationToken);

            return MapToDto(order);
        }



        public async Task<bool> UpdateAsync(string id, OrderDto orderDto)
        {
            var existing = await _orders.Find(x => x.OrderId == id).FirstOrDefaultAsync();
            if (existing == null)
                return false;

            var updatedOrder = MapToModel(orderDto);
            updatedOrder.OrderId = id;
            await _orders.ReplaceOneAsync(x => x.OrderId == id, updatedOrder);
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _orders.DeleteOneAsync(x => x.OrderId == id);
            return result.DeletedCount > 0;
        }

        #region Mapping
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
            OrderId = dto.OrderId,
            OrderedOn = dto.OrderedOn,
            OrderDetails = dto.OrderDetails?.Select(od => new OrderDetail
            {
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                Price = od.Price,
                UnitPrice = od.UnitPrice
            }).ToList()
        };
        #endregion
    }
}
