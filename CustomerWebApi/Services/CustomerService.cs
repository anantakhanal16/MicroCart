using CustomerWebApi.ApplicationDbContext;
using CustomerWebApi.Dto;
using CustomerWebApi.Interface;
using CustomerWebApi.Models;
using Messaging.Interfaces;
using Messaging.Models;
using Microsoft.EntityFrameworkCore;
using SharedContracts;

namespace CustomerWebApi.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _dbContext;
        private readonly IRabbitMqService _rabbitMqService;

        public CustomerService(AppDbContext dbContext,IRabbitMqService rabbitMqService)
        {
            _dbContext = dbContext;
            _rabbitMqService = rabbitMqService;
        }

        public async Task<List<Customer>> GetCustomersAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Customers.ToListAsync(cancellationToken);
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(c => c.customer_id == id, cancellationToken);
        }

        public async Task<Customer> CreateCustomerAsync(CustomerDto customerDto, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                customer_name = customerDto.customer_name,
                mobile_number = customerDto.mobile_number,
                email = customerDto.email
            };

            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return customer;
        }

        public async Task<Customer?> UpdateCustomerAsync(int id, CustomerDto customerDto, CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.customer_id == id, cancellationToken);

            if (customer == null)
                return null;

            customer.customer_name = customerDto.customer_name;
            customer.mobile_number = customerDto.mobile_number;
            customer.email = customerDto.email;

            _dbContext.Customers.Update(customer);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return customer;
        }

        public async Task<bool> DeleteCustomerAsync(int id, CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.customer_id == id, cancellationToken);

            if (customer == null)
                return false;

            _dbContext.Customers.Remove(customer);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> PublishOrderCreatedAsync(OrderCreatedEvent orderCreatedEvent, CancellationToken cancellationToken)
        {
            var orderCreated = new OrderCreatedEvent
            {
                Id = orderCreatedEvent.Id,
                CustomerName = orderCreatedEvent.CustomerName,
                ProductName = orderCreatedEvent.ProductName,
                Quantity = orderCreatedEvent.Quantity,
                Price = orderCreatedEvent.UnitPrice
            };

            var orderMessage = new RabbitMqMessage<OrderCreatedEvent>(
                message: orderCreated,
                exchange: "order.exchange",
                routingKey: "order.created",
                cancellationToken: cancellationToken
            );

            await _rabbitMqService.Publish(orderMessage,cancellationToken);
            return true;
        }
    }
}
