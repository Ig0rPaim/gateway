using BuilderAux.Models;
using BuilderAux.VOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BuilderAux.SevicesGateWay.Token
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenService()
        {
            
        }
        public string Generate(UsuariosVO user)
        {
            var builder = WebApplication.CreateBuilder();
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(builder
                .Configuration.GetSection("Keys")
                .GetSection("TokenKey")
                .Value ?? string.Empty) ;
            var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = credentials,
            };
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        private static ClaimsIdentity GenerateClaims(UsuariosVO user)
        {
            var ci = new ClaimsIdentity();
            ci.AddClaim(new Claim(ClaimTypes.Name, user.Email));
            ci.AddClaim(new Claim(ClaimTypes.Role, user.Role ?? "client"));
            return ci;
        }
    }
}
