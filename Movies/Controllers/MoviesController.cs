using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PostMovie()
        {
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> PutMovie()
        {
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMovie()
        {
            return Ok();
        }
    }
}
