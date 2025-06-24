using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Common;
using Movies.Models;
using Movies.Models.ViewModels;
using System.Linq;


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
        public async Task<ActionResult<Response<PaginatedResult<MovieViewModel>>>>GetMovies([FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 5)
        {
            var response = new Response<MovieViewModel>();

            if (pageIndex < 1  || pageSize < 1) 
            {
                response.Success = false;
                response.Message = "Page index and page size must be greater than 0.";
                response.Data = null;
                return BadRequest(response);

            }

            try 
            {

                IQueryable<MovieViewModel> query = _context.Movies.Include(m => m.IdGenreNavigation)
                    .Include(m => m.IdAgeRatingNavigation)
                    .Select(m => new MovieViewModel { 
                    IdMovie = m.IdMovie,
                    Name = m.Name,
                    IdGenre = m.IdGenreNavigation.IdGenre,
                    Genre = m.IdGenreNavigation.GenreName,
                    IdAgeRating = m.IdAgeRatingNavigation.IdAgeRating,
                    AgeRating = m.IdAgeRatingNavigation.RatingName,
                    ImageURL = m.ImageUrl,
                    DurationMinutes = m.DurationMinutes,
                    Resume = m.Resume,
                    ReleaseDate = m.RelaseDate


                  });
               
                var paginatedResult = await PaginatedResult<MovieViewModel>.CreateAsync(query, pageIndex, pageSize) ;

                response.Success = true;
                response.Message = "Movies retrieved successfully.";
                response.Data = paginatedResult;

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving movies.");
                response.Success = false;
                response.Message = ex + "An error occurred while retrieving movies.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
           
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Response<MovieViewModel>>> GetMovieByName(string name)
        {
            var response = new Response<MovieViewModel>();
            if (string.IsNullOrWhiteSpace(name)) 
            {
                response.Success = false;
                response.Message = "Movie name cannot be empty.";
                return BadRequest(response);
            }

            try 
            {
                var movie = await _context.Movies.Include(m => m.IdGenreNavigation)
                    .Include(m => m.IdAgeRatingNavigation)
                    .Where(m => m.Name.Contains(name))
                    .Select(m => new MovieViewModel
                    {
                        IdMovie = m.IdMovie,
                        Name = m.Name,
                        IdGenre = m.IdGenreNavigation.IdGenre,
                        Genre = m.IdGenreNavigation.GenreName,
                        IdAgeRating = m.IdAgeRatingNavigation.IdAgeRating,
                        AgeRating = m.IdAgeRatingNavigation.RatingName,
                        ImageURL = m.ImageUrl,
                        DurationMinutes = m.DurationMinutes,
                        Resume = m.Resume,
                        ReleaseDate = m.RelaseDate
                    })
                    .FirstOrDefaultAsync();
                if (movie == null) 
                {
                    response.Success = false;
                    response.Message = "Movie not found.";
                    return NotFound(response);
                }
                response.Success = true;
                response.Message = "Movie retrieved successfully.";
                response.Data = movie;
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the movie by name.");
                response.Success = false;
                response.Message = ex + "An error occurred while retrieving the movie by name.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostMovie(MovieViewModel model)
        {
            var response = new Response<MovieViewModel>();

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
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

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
            var response = new Response<MovieViewModel>();
            
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                   var movie = await _context.Movies.FindAsync(model.IdMovie);

                   if (movie == null)
                   {
                      response.Success = false;
                      response.Message = "Movie not found.";
                      return NotFound(response);
                   }

                    movie.Name = model.Name;
                    movie.IdGenre = model.IdGenre;
                    movie.IdAgeRating = model.IdAgeRating;
                    movie.ImageUrl = model.ImageURL;
                    movie.DurationMinutes = model.DurationMinutes;
                    movie.Resume = model.Resume;
                    movie.RelaseDate = model.ReleaseDate;

                    _context.Movies.Update(movie);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    response.Success = true;
                    response.Message = "Movie updated successfully.";

                    return Ok(response);

                }
                catch (Exception ex)
                {
                   transaction.Rollback();
                   _logger.LogError(ex, "An error occurred while updating the movie.");
                   response.Success = false;
                   response.Message = ex.Message;
                   return StatusCode(StatusCodes.Status500InternalServerError, response);
                }





            }
         
            
        }

        
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteMovie(string name)
        {
            var response = new Response<MovieViewModel>();

            using (var transaction = await _context.Database.BeginTransactionAsync()) 
            {

                try 
                {
                    var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Name == name);

                    if (movie == null) 
                    { 
                        response.Success = false;
                        response.Message = "Movie not found.";
                        return NotFound(response);

                    }
                    _context.Movies.Remove(movie);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    response.Success = true;
                    response.Message = "Movie deleted successfully.";
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while deleting the movie.");
                    response.Success = false;
                    response.Message = ex.Message;
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }



            }
            
        }
    }
}
