using Api.RequestHelper;
using Core.Entites;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected async Task<ActionResult> CreatePageResult<T>(
            IGenericRepository<T> repo,
            ISpecification<T> spec,
            int pageIndex,
            int pageSize) where T : BaseEntity
        {
            var items = await repo.GetListAsync(spec); 
            var count = await repo.CountAsync(spec); 
            var pagination = new Pagination<T>(pageIndex, pageSize, count, items!); 
            return Ok(pagination); 
        }
    }
}
