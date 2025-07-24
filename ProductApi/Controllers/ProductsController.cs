using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.ApplicationDbContext;
using ProductApi.Dto;
using ProductApi.Models;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductDbContext _productDbContext;

        public ProductsController(ProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductsDto dto)
        {
            var product = new Products
            {
                product_name = dto.product_name,
                product_code = dto.product_code,
                product_price = dto.product_price
            };

            _productDbContext.Products.Add(product);
            await _productDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new { id = product.product_id }, product);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productDbContext.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productDbContext.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductsDto dto)
        {
            var product = await _productDbContext.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            product.product_name = dto.product_name;
            product.product_code = dto.product_code;
            product.product_price = dto.product_price;

            await _productDbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productDbContext.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _productDbContext.Products.Remove(product);
            await _productDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
