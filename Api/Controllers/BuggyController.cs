using Api.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        [HttpGet("notfound")]
        public IActionResult GetNotFoundRequest()
        {
            return NotFound(new ProblemDetails { Title = "Not Found" });
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest(new ProblemDetails { Title = "Bad Request" });
        }

        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            return Unauthorized(new ProblemDetails { Title = "Unauthorized" });
        }

        [HttpPost("validationerror")]
        public IActionResult GetValidationError(CreateProductDto productDto)
        {
            return Ok(productDto);
        }

        [HttpGet("servererror")]
        public IActionResult GetServerError()
        {
            throw new Exception();
        }

        [HttpGet("testauth")]
        public IActionResult GetTestAuth()
        {
            return Ok(new { Message = "You are authenticated" });
        }


    }
}
