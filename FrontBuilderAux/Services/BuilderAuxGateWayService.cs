using FrontBuilderAux.Models;
using FrontBuilderAux.Utils;
using System.Runtime.CompilerServices;

namespace FrontBuilderAux.Services
{
    public class BuilderAuxGateWayService : IBuilderAuxGateWayService
    {
        private readonly HttpClient _httpClient;
        public const string BasePath = "api/Usuario";

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

        public async Task<Usuarios> PostAsync(Usuarios user)
        {
            var response = await _httpClient.PostAsJson(user, BasePath);
            if (response.IsSuccessStatusCode)
                return await response.ReadContetAs<Usuarios>();
            throw
                new Exception("fudeu!");
        }

        public async Task<Usuarios> PutAsync(string email, Usuarios user)
        {
            var response = await _httpClient.PutAsJson(user, BasePath);
            if (response.IsSuccessStatusCode)
                return await response.ReadContetAs<Usuarios>();
            throw
                new Exception("fudeu!");
        }
    }
}
