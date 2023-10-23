using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FrontBuilderAux.Services.Token
{
    public class DescripToken
    {
        private readonly IConfiguration _configuration;

        public DescripToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DescripToken()
        {

        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            if (string.IsNullOrEmpty(token)) { throw new ArgumentNullException("token"); }
            var builder = WebApplication.CreateBuilder();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Keys").GetSection("TokenKey").Value ?? string.Empty);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Token inválido");
            }

            return principal;
        }
    }
}
