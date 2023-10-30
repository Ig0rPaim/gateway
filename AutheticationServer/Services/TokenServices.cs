using AutheticationServer.Criptografia;
using AutheticationServer.DTOs;
using AutheticationServer.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AutheticationServer.Services
{
    public class TokenServices : ITokenServices
    {
        private JwtSecurityTokenHandler _tokenhandler;
        private byte[] _key;
        private WebApplicationBuilder _builder;
        private SecurityKey _keyIdentity;

        public TokenServices()
        {
            _tokenhandler = new JwtSecurityTokenHandler();
            _builder =  WebApplication.CreateBuilder();
            _key = Encoding.ASCII.GetBytes(_builder
                    .Configuration.GetSection("Keys")
                    .GetSection("TokenKey")
                    .Value ?? string.Empty);
            _keyIdentity = new SymmetricSecurityKey(_key);

        }

        public async Task<ResultCreate> CreateToken(UsuarioModel acesso, string code)
        {
            try
            {
                #region validação do clinte
                byte[] codeByte = Convert.FromBase64String(code);
                byte[] descrypCode = Asymmetrical.Descriptografar(codeByte);
                string stringCode = Convert.ToBase64String(descrypCode);
                string codeApplication = _builder
                    .Configuration.GetSection("Keys")
                    .GetSection("CodeBuilderAux")
                    .Value ?? throw new ArgumentNullException();
                if (stringCode != codeApplication) { throw new Exception("Cliente não Autorizado"); }
                #endregion

                #region Preechendo Claims
                ClaimsIdentity identity = new ClaimsIdentity(new Claim[] {
                        new Claim("Email", acesso.Email),
                        new Claim("Role", acesso.Role),
                        new Claim("Expires", acesso.Expires),
                        new Claim("Aplication", acesso.Aplication)
                }); ;
                #endregion

                #region Datas de criação e expiração
                DateTime createdDate = DateTime.UtcNow;
                DateTime expiresDate = createdDate +
                    TimeSpan.FromMinutes(Convert.ToInt32(acesso.Expires));
                #endregion

                #region Criação do token
                #region builder
                var builder = WebApplication.CreateBuilder();
                TokenModel tokenModel = new TokenModel
                {
                    Audience = code,
                    Issuer = builder.Configuration
                    .GetSection("Jwt")
                    .GetSection("Issuer")
                    .ToString() ?? string.Empty
                };
                #endregion
                var securityToken = _tokenhandler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenModel.Issuer,
                    Audience = tokenModel.Audience,
                    SigningCredentials = SymetricSecurityKey(),
                    Subject = identity,
                    NotBefore = createdDate,
                    Expires = expiresDate
                });
                #endregion

                var token = _tokenhandler.WriteToken(securityToken);

                #region Construindo retorno
                ResultCreate result = new ResultCreate
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
                ResultCreate result = new ResultCreate
                {
                    authenticated = false,
                    message = $"Falha na criação do Token: {ex.Message}"
                };
                return result;
            }
        }

        public async Task<ResultValidator> ValidateToken(string token, UsuarioModel acesso, string code)
        {
            try
            {
                #region validação do clinte
                byte[] codeByte = Convert.FromBase64String(code);
                byte[] descrypCode = Asymmetrical.Descriptografar(codeByte);
                string stringCode = Convert.ToBase64String(descrypCode);
                string codeApplication = _builder
                    .Configuration.GetSection("Keys")
                    .GetSection("CodeBuilderAux")
                    .Value ?? throw new ArgumentNullException();
                if (stringCode != codeApplication) { throw new Exception("Cliente não Autorizado"); }
                #endregion

                var tokenSecure = _tokenhandler.ReadToken(token) as SecurityToken;

                #region Validation Parameters
                #region builder
                var builder = WebApplication.CreateBuilder();
                TokenModel tokenModel = new TokenModel
                {
                    Audience = code,
                    Issuer = builder.Configuration
                    .GetSection("Jwt")
                    .GetSection("Issuer")
                    .ToString() ?? string.Empty
                };
                #endregion
                var _validationparameters = new TokenValidationParameters();
                _validationparameters.IssuerSigningKey = _keyIdentity;
                _validationparameters.ValidAudience = tokenModel.Audience;
                _validationparameters.ValidIssuer = tokenModel.Issuer;
                _validationparameters.ValidateIssuerSigningKey = true;
                _validationparameters.ValidateLifetime = true;
                _validationparameters.ClockSkew = TimeSpan.Zero;
                #endregion
                var teste = DateTime.UtcNow;
                if (tokenSecure != null && tokenSecure.ValidTo > DateTime.UtcNow)
                {
                    var claims = _tokenhandler.ValidateToken(token, _validationparameters, out tokenSecure);

                    string email = claims.FindFirstValue("Email") ?? throw new ArgumentNullException();
                    var role = claims?.FindFirstValue("Role") ?? throw new ArgumentNullException();
                    var aplication = claims?.FindFirstValue("Aplication") ?? throw new ArgumentNullException();

                    if (email == acesso.Email && role == acesso.Role && aplication == acesso.Aplication)
                    {
                        var validto = tokenSecure.ValidTo.ToLocalTime();

                        #region contruindo retorno
                        ResultValidator result = new ResultValidator
                        {
                            authenticated = true,
                            lifetime = validto.ToString("yyyy-MM-dd HH:mm:ss"),
                            accessToken = token,
                            message = "OK"
                        };
                        #endregion
                        return result;
                    }
                    else
                    {
                        #region contruindo retorno
                        ResultValidator result = new ResultValidator
                        {
                            authenticated = false,
                            message = "Token informado não pode ser utilizado para a requisição"
                        };
                        #endregion
                        return result;
                    }
                }
                else
                {
                    #region contruindo retorno
                    ResultValidator result = new ResultValidator
                    {
                        authenticated = false,
                        message = "Token expirado e não pode ser utilizado"
                    };
                    #endregion
                    return result;
                }
            }
            catch (Exception ex)
            {
                #region contruindo retorno
                ResultValidator result = new ResultValidator
                {
                    authenticated = false,
                    message = $"Falha na validação do Token: {ex.Message}"
                };
                #endregion
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

