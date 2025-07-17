using Api.RequestHelper;
using Core.Entites;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IGenericRepository<Product> _genericRepository;
        public ProductController(IGenericRepository<Product> productRepository)
        {
            _genericRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] ProductSpecParams productSpec)
        {
            var spec = new ProductSpecification(productSpec);
            return await CreatePageResult(_genericRepository, spec, productSpec.PageIndex, productSpec.PageSize);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _genericRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            var created = await _genericRepository.AddAsync(product);
            return Ok(created);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct(Product product)
        {
            if (!await _genericRepository.ExistsAsync(product.Id))
                return BadRequest("Product Not Found.");

            await _genericRepository.UpdateAsync(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _genericRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            await _genericRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecfication();
            //return Ok(await _genericRepository.GetListAsync(spec));
            return Ok((await _genericRepository.GetListAsync<string>(spec)).Distinct().ToList());

        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            //return Ok(await _genericRepository.GetListAsync(spec));
            return Ok((await _genericRepository.GetListAsync<string>(spec)).Distinct().ToList());

        }
    }
}
