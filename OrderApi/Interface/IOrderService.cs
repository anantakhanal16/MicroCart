using OrderApi.Dtos;

namespace OrderApi.Interface
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllAsync();
        Task<OrderDto> GetByIdAsync(string id);
        Task<OrderDto> CreateAsync(OrderDto orderDto, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(string id, OrderDto orderDto);
        Task<bool> DeleteAsync(string id);
    }
}
