using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Movies.Common;
using Movies.Models;
using Movies.Models.ViewModels;
using Movies.Services;

namespace Movies.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private IAuthService _authService;
        private readonly MoviesContext _context;
        public UsersController(ILogger<UsersController> logger, IAuthService authService, MoviesContext context)
        {
            _logger = logger;
            _authService = authService;
            _context = context;
        }

        //This need testing

        [HttpPost("Login")]
        public IActionResult Login([FromBody] AuthViewModel model)
        {
            var response = new Response<User>();

            var userResponse = _authService.Auth(model);

            if (userResponse == null) 
            {
                response.Success = false;
                response.Message = "Invalid email or password.";
                return Unauthorized(response);

            }

            response.Success = true;
            response.Message = "Login successful.";
            response.Data = userResponse;

            return Ok(response);
        }


        // This need testing
        [HttpPost("Register")]

        public async Task<IActionResult> Register(UserViewModel model) 
        {




            return Ok();
        }

    }
}
