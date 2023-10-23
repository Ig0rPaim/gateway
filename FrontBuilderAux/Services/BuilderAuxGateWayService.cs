using FrontBuilderAux.DTOs;
using FrontBuilderAux.Models;
using FrontBuilderAux.Services.Token;
using FrontBuilderAux.Utils;
using System.Security.Claims;

namespace FrontBuilderAux.Services
{
    public class BuilderAuxGateWayService : IBuilderAuxGateWayService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string basePath = "https://localhost:7148/";
        private const string BasePath = "api/Usuario";
        private const string BasePathToLogin = "api/Usuario/Login";

        public BuilderAuxGateWayService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            //_httpContext = httpContext;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException();
        }

        public async Task<bool> DeleteAsync(string email)
        {
            var response = await _httpClient.DeleteAsync(BasePath + email);
            if (response.IsSuccessStatusCode)
                return await response.ReadContetAs<bool>();
            throw
                new Exception("q merda, hein");
        }

        public async Task<Dictionary<string, string>> GetAsync()
        {
            var response = await _httpClient.GetAsync(BasePath);
            return await response.ReadContetAs<Dictionary<string, string>>();
        }

        public async Task<Dictionary<string, string>> GetByEmailAsync(string email)
        {
            var response = await _httpClient.GetAsync(BasePath + email);
            return await response.ReadContetAs<Dictionary<string, string>>();
        }

        public Task MudarSenha(string novaSenha, string email)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> PostAsync(UsuariosCreateAccount user)
        {
            var response = await _httpClient.PostAsJson(user, BasePath);
            var result = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode) return true;
            if (response.ReasonPhrase != null) throw new Exception(result);
            throw new Exception("fudeu!");
        }

        public async Task<Usuarios> PutAsync(string email, Usuarios user)
        {
            var response = await _httpClient.PutAsJson(user, BasePath);
            if (response.IsSuccessStatusCode)
                return await response.ReadContetAs<Usuarios>();
            throw
                new Exception("fudeu!");
        }

        public async Task<bool> Login(UsuariosLogin user, HttpContext context)
         {
            try
            {
                var response = await _httpClient.PostAsJson(user, "https://localhost:7148/api/Usuario/Login");
                var headers = response.Headers;
                var TokenAuth = headers.GetValues("TokenAuth");
                var emailSession = headers.GetValues("Email");
                var senhaSession = headers.GetValues("Senha");
                var roleSession = headers.GetValues("Role");
                var telefoneSession = headers.GetValues("Telefone");
                string Auth = string
                    .Concat(
                    emailSession.FirstOrDefault(), ", ",
                    senhaSession.FirstOrDefault(), ", ",
                    roleSession.FirstOrDefault(), ", ",
                    telefoneSession.FirstOrDefault(), ", ",
                    TokenAuth.FirstOrDefault(), ", "
                    );

                //DescripToken descripToken = new DescripToken();
                //var dataUser = descripToken.GetPrincipalFromToken(TokenAuth.FirstOrDefault() ?? throw new ArgumentNullException());
                ////dataUser.FindAll();
                //string email = dataUser.Identity?.Name ?? throw new ArgumentNullException("Cadê seu Email, viado?");
                //string role = dataUser.FindFirst(ClaimTypes.Role)?.ToString() ?? throw new ArgumentNullException("sim, calabreso, qual seu cargo?!");
                //List<string> dataSession = new List<string> { email, role};
                //string datas = string.Join(", ", dataSession);
                //_httpContext.Session.SetString("DataUser", datas);
                context.Session.SetString("DataUser", Auth);
                var result = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode) return true;
                else
                {
                    throw new Exception(result);
                }
            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }
        }
    }
}
