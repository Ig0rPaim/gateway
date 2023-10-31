using BuilderAux.Criptografia;
using BuilderAux.DTO_s;
using BuilderAux.Utils;

namespace BuilderAux.SevicesGateWay.AutheticationServer
{
    public class AuthenticationServer : IAuthenticationServer
    {
        private readonly HttpClient _httpClient;
        private const string BasePath = "https://localhost:7224/token";
        private static readonly WebApplicationBuilder builder = WebApplication.CreateBuilder();
        //private CriptografiaAssimetrica _criptografia = new Criptografia.CriptografiaAssimetrica();

        public AuthenticationServer(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            //_criptografia = criptografia ?? throw new ArgumentNullException();
        }

        public async Task<ResultCreateToken> CreateToken(Authentication acesso, HttpContext context)
        {
            try
            {
                string path = $"{BasePath}/gerar";
                string code = builder.Configuration
                    .GetSection("Keys")
                    .GetSection("AuthenticationKey")
                    .Value ?? throw
                    new ArgumentNullException("Erro ao encontrar código de verificação");
                byte[] codeByte = Convert.FromBase64String(code);
                codeByte = CriptografiaAssimetrica.Criptografar(codeByte);
                code = Convert.ToBase64String(codeByte);
                _httpClient.DefaultRequestHeaders.Add("Code", code);
                var response = await _httpClient.PostAsJson(acesso, path);
                var headers = response.Headers;
                if (headers.GetValues("authenticated").FirstOrDefault() == "false") throw new UnauthorizedAccessException("Usuario não Autorizado");
                string? TokenAuth = headers.GetValues("TokenAuth").FirstOrDefault();
                string? created = headers.GetValues("created").FirstOrDefault();
                string? expiration = headers.GetValues("expiration").FirstOrDefault();
                ResultCreateToken authentication = new ResultCreateToken
                {
                    created = created,
                    expiration = expiration,
                    accessToken = TokenAuth,
                    email = acesso.Email,
                    role = acesso.Role,
                };
                return await Task.FromResult(authentication);
            }
            catch (Exception er)
            {

                throw new Exception(er.Message);
            }
        }

        public async Task<ResultValidateToken> ValidateToken(Authentication acesso)
        {
            string path = BasePath + "/validar";
            var response  = await _httpClient.PostAsJson(acesso, path);
            var headers = response.Headers;
            if (headers.GetValues("authenticated").FirstOrDefault() == "false") throw new UnauthorizedAccessException("Usuario não autorizado");
            string? lifetime = headers.GetValues("lifetime").FirstOrDefault();
            string? accessToken = headers.GetValues("accessToken").FirstOrDefault();

            ResultValidateToken authentication = new ResultValidateToken
            {
                LifeTime = lifetime,
                AccessToken = accessToken,
                Email = acesso.Email,
                Role = acesso.Role,
            };
            return await Task.FromResult(authentication);
        }
    }
}
