using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Common;
using Movies.Models;
using Movies.Models.ViewModels;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly MoviesContext _context;    

        public MoviesController(ILogger<MoviesController> logger, MoviesContext context)
        {
            _logger = logger;
            _context = context;
        }





        [HttpGet]
        public async Task<ActionResult<Response<PaginatedResult<Movie>>>>GetMovies([FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 5)
        {
            var response = new Response<Movie>();

            if (pageIndex < 1  || pageSize < 1) 
            {
                response.Success = false;
                response.Message = "Page index and page size must be greater than 0.";
                response.Data = null;
                return BadRequest(response);

            }

            try 
            {
                IQueryable<Movie> query = _context.Movies.OrderBy(m => m.IdMovie);
                var paginatedResult = await PaginatedResult<Movie>.CreateAsync(query, pageIndex, pageSize);

                response.Success = true;
                response.Message = "Movies retrieved successfully.";
                response.Data = paginatedResult;

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving movies.");
                response.Success = false;
                response.Message = "An error occurred while retrieving movies.";
                response.Data = null;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> PostMovie(MovieViewModel model)
        {
            var response = new Response<Movie>();

            using (var transaction = await _context.Database.BeginTransactionAsync()) 
            {

                try
                {
                    Movie movie = new Movie();

                    movie.Name = model.Name;
                    movie.IdGenre = model.IdGenre;
                    movie.IdAgeRating = model.IdAgeRating;
                    movie.ImageUrl = model.ImageURL;
                    movie.DurationMinutes = model.DurationMinutes;
                    movie.Resume = model.Resume;
                    movie.RelaseDate = model.ReleaseDate;

                    _context.Movies.Add(movie); 
                    _context.SaveChanges();
                    transaction.Commit();

                    response.Success = true;
                    response.Message = "Movie added successfully.";


                    return Ok(response);

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "An error occurred while adding the movie.");
                    response.Success = false;
                    response.Message = ex.Message;
                    return StatusCode(StatusCodes.Status500InternalServerError, response);

                }

            }
           
         
        }
        [HttpPut]
        public async Task<IActionResult> PutMovie(MovieViewModel model)
        {
            var response = new Response<Movie>();
            try 
            {
            
            
            
            
            
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the movie.");
                response.Success = false;
                response.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMovie()
        {
            return Ok();
        }
    }
}
