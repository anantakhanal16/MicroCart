using CustomerWebApi.Dto;
using CustomerWebApi.Models;
using SharedContracts;

namespace CustomerWebApi.Interface
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetCustomersAsync(CancellationToken cancellationToken);
        Task<Customer?> GetCustomerByIdAsync(int id, CancellationToken cancellationToken);
        Task<Customer> CreateCustomerAsync(CustomerDto customerDto, CancellationToken cancellationToken);
        Task<Customer?> UpdateCustomerAsync(int id, CustomerDto customerDto, CancellationToken cancellationToken);
        Task<bool> DeleteCustomerAsync(int id, CancellationToken cancellationToken);
        Task<bool> PublishOrderCreatedAsync(OrderCreatedEvent orderCreatedEvent, CancellationToken cancellationToken);
    }
}
