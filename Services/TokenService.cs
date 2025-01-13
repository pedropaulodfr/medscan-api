using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using authentication_jwt.DTO;
using authentication_jwt.Models;
using Microsoft.IdentityModel.Tokens;

namespace authentication_jwt.Services
{
    public static class TokenService
    {
        public static string GenerateToken(UsuarioDTO user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Nome.ToString()),
                    new Claim("Perfil", user.Perfil.ToString()),
                    new Claim("Email", user.Email.ToString()),
                    new Claim("UsuarioId", user.Id.ToString()),
                    new Claim("PacienteId", user.PacienteId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(Settings.ExpiryTimeInHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = Settings.Audience,
                Issuer = Settings.Issuer
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static User ValidarTokenJWT(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Settings.Secret);

                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var tokenUsername = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                var tokenRole = claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value;

                User user = new User{
                    Username = tokenUsername,
                    Role = tokenRole,
                    Token = token,
                };

                return user;
            }
            catch
            {
                return null;
            }
        }
    }
}