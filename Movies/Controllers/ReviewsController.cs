using Microsoft.AspNetCore.Mvc;
using Movies.Models;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ILogger<ReviewsController> _logger;
        private readonly MoviesContext _context;

        public ReviewsController( MoviesContext moviesContext, ILogger<ReviewsController> logger)
        {
            _context = moviesContext;
            _logger = logger;

        }




    }
}
