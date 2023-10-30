using BuilderAux.DTO_s;
using BuilderAux.Utils;

namespace BuilderAux.SevicesGateWay.AutheticationServer
{
    public class AuthenticationServer : IAuthenticationServer
    {
        private readonly HttpClient _httpClient;
        private const string BasePath = "https://localhost:7224";

        public AuthenticationServer(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<ResultCreateToken> CreateToken(Authentication acesso, HttpContext context)
        {
            string path = BasePath + "/gerar";
            var setHeaders = context.Response;
            setHeaders.Headers.Add("Code", "h2m/p6Ca4x9UWru3ug8vDXoAcpaWm8D/Hzvc3YTq3KV5j1DC/jJE8TK+OQ2suWSkzEPVhuGBGBX62SvYSgn5HHj6ChueCvLJpzYylUSpjB2bRMj6clpKAC3XOVuvcTBU/W5+Ej/Z4eJ+7I/DCE0d1Iw01jML59FN6++4CxXyuymcTq4XQoTjW0J7kuIlUiwm7RuDhScNjKUIWjcOHPeCI8NKhA2kwmOTVfvrkKlOGiVXw5U1J/iWS7KX560lhdOuGFvhvZPPN9xlikravMbY5DEDrr3SyIgAIiD/750gd7vspT98pUpykMqb3A+tnIH069uhrhjmKzP5zSaMJlNoTw==");
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
