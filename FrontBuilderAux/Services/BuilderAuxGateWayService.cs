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
        private const string BasePath = "api/Usuario";
        private const string BasePathToLogin = "api/Usuario/Login";

        public BuilderAuxGateWayService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
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

        public async Task<bool> Login(UsuariosLogin user)
         {
            try
            {
                var response = await _httpClient.PostAsJson(user, BasePathToLogin);
                var headers = response.Headers;
                var TokenAuth = headers.GetValues("TokenAuth");
                DescripToken descripToken = new DescripToken();
                var dataUser = descripToken.GetPrincipalFromToken(TokenAuth.ToString() ?? string.Empty);
                var email = dataUser.Identity?.Name;
                var role = dataUser.FindFirst(ClaimTypes.Role)?.ToString();
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
