using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Movies.Common
{
    public class AuthService
    {

        private readonly string _configuration; // Almacena la clave secreta para firmar los JWT

        // El constructor recibe IConfiguration para obtener la clave secreta del JWT
        public AuthService(IConfiguration configuration)
        {
            // Obtiene la clave secreta de la sección "AppSettings:Secret" de tu appsettings.json
            _configuration = configuration["AppSettings:Secret"] ??
                         throw new ArgumentNullException("AppSettings:Secret", "JWT Secret is not configured.");
        }

        /// <summary>
        /// Hashea una contraseña en texto plano usando BCrypt.
        /// Este método se usa cuando se registra un nuevo usuario o se actualiza una contraseña.
        /// </summary>
        /// <param //name="password">La contraseña en texto plano a hashear.</param>
        /// <returns>La contraseña hasheada y salteada por Bcrypt.</returns>
        public string HashPassword(string password)
        {
            // El 'workFactor' (también llamado 'cost') determina cuántas iteraciones se realizan.
            // Un valor más alto hace que el hash sea más lento y seguro.
            // 12 es un buen punto de partida seguro y eficiente.
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        /// <summary>
        /// Verifica una contraseña en texto plano contra un hash de Bcrypt almacenado.
        /// Este método se usa durante el proceso de login.
        /// </summary>
        /// <param // name="password">La contraseña en texto plano proporcionada por el usuario.</param>
        /// <param //name="hashedPassword">La contraseña hasheada (de Bcrypt) almacenada en la base de datos.</param>
        /// <returns>True si la contraseña coincide con el hash, False en caso contrario.</returns>
        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Bcrypt se encarga automáticamente de extraer el salt y el workFactor del hashedPassword
            // para realizar la verificación.
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        /// <summary>
        /// Genera un JSON Web Token (JWT) para un usuario autenticado.
        /// </summary>
        /// <param// name="userId">El ID único del usuario.</param>
        /// <param// name="email">El email del usuario (o username, para incluir en los claims).</param>
        /// <param //name="role">El rol del usuario (opcional).</param>
        /// <returns>El JWT generado como una cadena.</returns>
        public string GenerateJwtToken(int userId, string email, string role = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration); // Convierte la clave secreta a bytes

            // Define los "claims" (afirmaciones) que se incluirán en el token.
            // Estos claims representan la identidad y permisos del usuario.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()), // ID del usuario
                new Claim(ClaimTypes.Email, email) // Email del usuario
            };

            // Añade el rol si está presente
            if (!string.IsNullOrEmpty(role))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Define las propiedades del token, como su sujeto, expiración y credenciales de firma
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), // El conjunto de claims
                Expires = DateTime.UtcNow.AddMinutes(10), // El token expira en 10 minutos (ajustable)
                // Credenciales para firmar el token, usando HMAC SHA256 con tu clave secreta
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // Crea y escribe el token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
