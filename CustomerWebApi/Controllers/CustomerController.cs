using CustomerWebApi.ApplicationDbContext;
using CustomerWebApi.Dto;
using CustomerWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public CustomerController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/customer
        [HttpGet]
        public async Task<IActionResult> GetCustomers(CancellationToken cancellationToken)
        {
            try
            {
                var customers = await _dbContext.Customers.ToListAsync(cancellationToken);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        // GET: api/customer/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _dbContext.Customers
                    .FirstOrDefaultAsync(c => c.customer_id == id, cancellationToken);

                if (customer == null)
                    return NotFound(new { message = $"Customer with ID '{id}' not found." });

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        // POST: api/customer
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto customerDto, CancellationToken cancellationToken)
        {
            try
            {
                if (customerDto == null)
                    return BadRequest(new { message = "Invalid customer data." });

                var customer = new Customer
                {
                    customer_name = customerDto.customer_name,
                    mobile_number = customerDto.mobile_number,
                    email = customerDto.email
                };

                _dbContext.Customers.Add(customer);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return CreatedAtAction(nameof(GetCustomerById), new { id = customer.customer_id }, customer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating customer: {ex.Message}");
            }
        }

        // PUT: api/customer/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDto updatedCustomerDto, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.customer_id == id, cancellationToken);

                if (customer == null)
                    return NotFound(new { message = $"Customer with ID '{id}' not found." });

                customer.customer_name = updatedCustomerDto.customer_name;
                customer.mobile_number = updatedCustomerDto.mobile_number;
                customer.email = updatedCustomerDto.email;

                _dbContext.Customers.Update(customer);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating customer: {ex.Message}");
            }
        }

        // DELETE: api/customer/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.customer_id == id, cancellationToken);

                if (customer == null)
                    return NotFound(new { message = $"Customer with ID '{id}' not found." });

                _dbContext.Customers.Remove(customer);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Ok(new { message = $"Customer with ID '{id}' deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting customer: {ex.Message}");
            }
        }
    }
}
