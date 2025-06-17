using Core.Entites;
using Infrastructure.Data;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly StoreContext _storeContext;
        public ProductController(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _storeContext.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _storeContext.Products.FindAsync(id);

            if (product == null) return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            await _storeContext.Products.AddAsync(product);

            await _storeContext.SaveChangesAsync();

            return Ok(product);
        }

        private bool ProductExist(int id)
        {
            return _storeContext.Products.Any(x => x.Id == id);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct(Product product)
        {
            if (!ProductExist(product.Id)) 
                return BadRequest("Product Not Found."); 

            _storeContext.Products.Update(product);
            await _storeContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> UpdateProduct(int id)
        {
            var product = await _storeContext.Products.FindAsync(id);
            if (product == null) return NotFound();

            _storeContext.Products.Remove(product);
            await _storeContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
    