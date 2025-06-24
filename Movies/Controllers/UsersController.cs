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
            try 
            {
              
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
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred during login.");
                response.Success = false;
                response.Message = ex + "An error occurred during login. Please try again later.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            
        }


        // This need testing
        [HttpPost("Register")]

        public async Task<IActionResult> Register(UserViewModel model) 
        {

            using (var transaction = await _context.Database.BeginTransactionAsync()) 
            {
                var response = new Response<User>();
                try
                {

                    if (_context.Users.Any(u => u.Email == model.Email))
                    {

                        throw new Exception("Email already exists.");

                    }

                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                    var user = new User();

                    user.FullName = model.FullName;
                    user.Email = model.Email;
                    user.Password = hashedPassword;
                    
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    response.Success = true;
                    response.Message = "User registered successfully.";
                    
                    return Ok(response);

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "An error occurred during user registration.");
                    response.Success = false;
                    response.Message = ex + "An error occurred during user registration. Please try again later.";
                    return StatusCode(StatusCodes.Status500InternalServerError, response);

                }




            }


           
        }

    }
}
