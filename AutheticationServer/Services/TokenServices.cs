using AutheticationServer.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AutheticationServer.Services
{
    public class TokenServices : ITokenServices
    {
        private TokenModel _tokenconfiguration;
        private JwtSecurityTokenHandler _tokenhandler;
        private byte[] _key;
        private WebApplicationBuilder _builder;
        private SecurityKey _keyIdentity;

        public TokenServices()
        {
            _tokenconfiguration = new TokenModel();
            _tokenhandler = new JwtSecurityTokenHandler();
            _builder =  WebApplication.CreateBuilder();
            _key = Encoding.ASCII.GetBytes(_builder
                    .Configuration.GetSection("Keys")
                    .GetSection("TokenKey")
                    .Value ?? string.Empty);
            _keyIdentity = new SymmetricSecurityKey(_key);

        }

        public async Task<object> CreateToken(UsuarioModel acesso)
        {
            try
            {
                #region Preechendo Claims
                ClaimsIdentity identity = new ClaimsIdentity(new Claim[] {
                        new Claim("Email", acesso.Email),
                        new Claim("Role", acesso.Role),
                        new Claim("Expires", acesso.Expires)
                });
                #endregion

                #region Datas de criação e expiração
                DateTime createdDate = DateTime.Now;
                DateTime expiresDate = createdDate +
                    TimeSpan.FromMinutes(Convert.ToInt32(acesso.Expires));
                #endregion

                #region Criação do token
                var securityToken = _tokenhandler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = _tokenconfiguration.Issuer,
                    Audience = _tokenconfiguration.Audience,
                    SigningCredentials = SymetricSecurityKey(),
                    Subject = identity,
                    NotBefore = createdDate,
                    Expires = expiresDate
                });
                #endregion

                var token = _tokenhandler.WriteToken(securityToken);

                #region Construindo retorno
                var result = new
                {
                    authenticated = true,
                    created = createdDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = expiresDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = token,
                    message = "OK"
                };
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                var result = new
                {
                    authenticated = false,
                    message = $"Falha na criação do Token: {ex.Message}"
                };

                return result;
            }
        }

        public async Task<object> ValidateToken(string token, UsuarioModel acesso)
        {
            try
            {
                var tokenSecure = _tokenhandler.ReadToken(token) as SecurityToken;

                #region Validation Parameters
                var _validationparameters = new TokenValidationParameters();
                _validationparameters.IssuerSigningKey = _keyIdentity;
                _validationparameters.ValidAudience = _tokenconfiguration.Audience;
                _validationparameters.ValidIssuer = _tokenconfiguration.Issuer;
                _validationparameters.ValidateIssuerSigningKey = true;
                _validationparameters.ValidateLifetime = true;
                _validationparameters.ClockSkew = TimeSpan.Zero;
                #endregion

                if (tokenSecure != null && tokenSecure.ValidTo > DateTime.UtcNow)
                {
                    var claims = _tokenhandler.ValidateToken(token, _validationparameters, out tokenSecure);

                    string email = claims.FindFirstValue("Email") ?? throw new ArgumentNullException();
                    var role = claims?.FindFirstValue("Role") ?? throw new ArgumentNullException();
                    var aplication = claims?.FindFirstValue("Aplication") ?? throw new ArgumentNullException();

                    if (email == acesso.Email && role == acesso.Role && aplication == acesso.Aplication)
                    {
                        var validto = tokenSecure.ValidTo.ToLocalTime();

                        var result = new
                        {
                            authenticated = true,
                            lifetime = validto.ToString("yyyy-MM-dd HH:mm:ss"),
                            accessToken = token,
                            message = "OK"
                        };
                        return result;
                    }
                    else
                    {
                        var result = new
                        {
                            authenticated = false,
                            message = "Token informado não pode ser utilizado para a requisição"
                        };

                        return result;
                    }
                }
                else
                {
                    var result = new
                    {
                        authenticated = false,
                        message = "Token expirado e não pode ser utilizado"
                    };

                    return result;
                }
            }
            catch (Exception ex)
            {
                var result = new
                {
                    authenticated = false,
                    message = $"Falha na validação do Token: {ex.Message}"
                };

                return result;
            }
        }

        private SigningCredentials SymetricSecurityKey()
        {
            try
            {
                var credentials = new SigningCredentials(
                new SymmetricSecurityKey(_key),
                SecurityAlgorithms.HmacSha256Signature);
                return credentials;
            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }
        }
    }

 }

