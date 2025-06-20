using Microsoft.AspNetCore.Mvc;
using Movies.Models;

namespace Movies.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly MoviesContext _context;
        public UsersController(ILogger<UsersController> logger, MoviesContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public IActionResult Login()
        {

            try 
            {
            
            }catch (Exception ex)
            {
               
            }
            return View();
        }
    }
}
