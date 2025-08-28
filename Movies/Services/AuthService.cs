using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Movies.Common;
using Movies.Models;
using Movies.Models.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

      

        public async Task<UserResponse> ValidateRefreshToken(string refreshToken, string email)
        {
            var tokenEntity = await _context.RefreshTokens
                .Include(rt => rt.IdUserNavigation)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.IdUserNavigation.Email == email  &&  !rt.Revoked);

            if(tokenEntity == null || tokenEntity.ExpirationDate < DateTime.UtcNow)
                return null; 

            tokenEntity.Revoked = true;
            _context.RefreshTokens.Update(tokenEntity);

            var accessToken = GenerateJwtToken(tokenEntity.IdUserNavigation);

            var newRefreshToken = RefreshTokenGenerator.GenerateRefreshToken();

            var newTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                CreationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddDays(7), 
                Revoked = false,
                IdUser = tokenEntity.IdUserNavigation.IdUser
            };

            await _context.RefreshTokens.AddAsync(newTokenEntity);
            await _context.SaveChangesAsync();

            return new UserResponse
            {
                Email = tokenEntity.IdUserNavigation.Email,
                Token = accessToken,
                RefreshToken = newRefreshToken

            };



        }

        public async Task<UserResponse>Auth(AuthViewModel model) 
        { 
                
            var userResponse = new UserResponse();

            var  user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
     
            if (user == null ||  string.IsNullOrEmpty(user.Password))
            {
                return null; 
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);

            if (!isValidPassword)
            {
                return null;
            }

            var refreshToken = new RefreshToken();

            refreshToken.Token = RefreshTokenGenerator.GenerateRefreshToken();
            refreshToken.CreationDate = DateTime.UtcNow;
            refreshToken.ExpirationDate = DateTime.UtcNow.AddDays(7); // Set expiration date to 7 days from now
            refreshToken.Revoked = false;
            refreshToken.IdUser = user.IdUser;
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            userResponse.Email = user.Email;
            userResponse.Token = GenerateJwtToken(user);
            userResponse.RefreshToken = refreshToken.Token;

            return userResponse;
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret); // Convierte la clave secreta a bytes

            //Para obtener los roles del usuario
            var userRoles = _context.UsersRoles.Where(ur => ur.IdUser == user.IdUser)
                            .Join(_context.Roles, ur => ur.IdRole, r => r.IdRole, (ur, r) => r.RoleName).ToList();

            //Permisos asociados a los roles del usuario

            var rolesId = _context.UsersRoles.Where(ur => ur.IdUser == user.IdUser).Select(ur => ur.IdRole).ToList();

            //Permisos unico asociados a los roles del usuario

            var userPermissions = _context.RolesPermissions.Where(rp => rolesId.Contains(rp.IdRole))
                                   .Include(rp => rp.IdPermissionNavigation).Select(rp => rp.IdPermissionNavigation.PermissionName)
                                   .Distinct().ToList();


            var Claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.IdUser.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.FullName),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Iss, _appSettings.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud, _appSettings.Audience)
            };

            foreach (var role in userRoles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(Claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public async Task<bool> RevokeRefreshToken(int userId)
        {
            var tokens = await _context.RefreshTokens.Where(r => r.IdUser == userId && !r.Revoked).ToListAsync();
           
            foreach (var token in tokens)
            {
                token.Revoked = true;
                _context.RefreshTokens.Update(token);
            }
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
