using FrontBuilderAux.DTOs;
using FrontBuilderAux.Models;

namespace FrontBuilderAux.Services
{
    public interface IBuilderAuxGateWayService
    {
        public Task<Dictionary<string, string>> GetAsync();
        public Task<Dictionary<string, string>> GetByEmailAsync(string email);
        public Task<bool> PostAsync(UsuariosCreateAccount user);
        public Task<Usuarios> PutAsync(string email, Usuarios user);
        public Task<bool> DeleteAsync(string email);
        public Task MudarSenha(string novaSenha, string email);
        public Task<bool> Login(UsuariosLogin user, HttpContext context);
    }
}
