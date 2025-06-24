using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Movies.Common;
using Movies.Models;
using Movies.Models.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Movies.Services
{
    public class AuthService : IAuthService
    {

       private readonly MoviesContext _context;
        private readonly AppSettings _appSettings;


        public AuthService(MoviesContext context, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _context = context;
            
        }



        public UserResponse Auth(AuthViewModel model) 
        { 
            UserResponse userResponse = new UserResponse();


            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
     
            if (user == null ||  string.IsNullOrEmpty(user.Password))
            {
                return null; 
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);

            if (!isValidPassword)
            {
                return null;
            }

            userResponse.Email = user.Email;
            userResponse.Token = GenerateJwtToken(user);

            return userResponse;
        }
        

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret); // Convierte la clave secreta a bytes

            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(new Claim[] 
                {
                     new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                     new Claim(ClaimTypes.Email, user.Email), 
                }), 

                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
