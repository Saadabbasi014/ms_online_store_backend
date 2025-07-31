using Core.Entites;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ProductController(IUnitOfWork unit) : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] ProductSpecParams productSpec)
        {
            var spec = new ProductSpecification(productSpec);
            return await CreatePageResult(unit.Repository<Product>(), spec, productSpec.PageIndex, productSpec.PageSize);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await unit.Repository<Product>().GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            await unit.Repository<Product>().AddAsync(product);
            if(await unit.Complete()) return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            return BadRequest("Problem creating product");
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct(Product product)
        {
            if (!await unit.Repository<Product>().ExistsAsync(product.Id))
                return BadRequest("Product Not Found.");

            await unit.Repository<Product>().UpdateAsync(product);

            if (await unit.Complete()) return NoContent();
            return BadRequest("Problem creating product");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await unit.Repository<Product>().GetByIdAsync(id);
            if (product == null) return NotFound();

            await unit.Repository<Product>().DeleteAsync(id);
            if (await unit.Complete()) return NoContent();
            return BadRequest("Problem deleting product");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecfication();
            //return Ok(await _genericRepository.GetListAsync(spec));
            return Ok((await unit.Repository<Product>().GetListAsync<string>(spec)).Distinct().ToList());

        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            //return Ok(await _genericRepository.GetListAsync(spec));
            return Ok((await unit.Repository<Product>().GetListAsync<string>(spec)).Distinct().ToList());

        }
    }
}
