using System.Threading;
using CustomerWebApi.Dto;
using CustomerWebApi.Interface;
using Messaging.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedContracts;

namespace CustomerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
       

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
          
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCustomers(CancellationToken cancellationToken)
        {
            var customers = await _customerService.GetCustomersAsync(cancellationToken);
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id, CancellationToken cancellationToken)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id, cancellationToken);
            if (customer == null)
                return NotFound(new { message = $"Customer with ID '{id}' not found." });

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto customerDto, CancellationToken cancellationToken)
        {
            if (customerDto == null)
                return BadRequest(new { message = "Invalid customer data." });

            var customer = await _customerService.CreateCustomerAsync(customerDto, cancellationToken);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.customer_id }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDto customerDto, CancellationToken cancellationToken)
        {
            var updatedCustomer = await _customerService.UpdateCustomerAsync(id, customerDto, cancellationToken);
            if (updatedCustomer == null)
                return NotFound(new { message = $"Customer with ID '{id}' not found." });

            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id, CancellationToken cancellationToken)
        {
            var deleted = await _customerService.DeleteCustomerAsync(id, cancellationToken);
            if (!deleted)
                return NotFound(new { message = $"Customer with ID '{id}' not found." });

            return Ok(new { message = $"Customer with ID '{id}' deleted successfully." });
        }

        [HttpPost("create-order")]
        public async Task<IActionResult>  CreateOrder([FromBody] OrderCreatedEvent order,CancellationToken cancellationToken)
        {
            var publishedOrder = await _customerService.PublishOrderCreatedAsync(order, cancellationToken);
            return Ok(publishedOrder);
        }
    }
}
