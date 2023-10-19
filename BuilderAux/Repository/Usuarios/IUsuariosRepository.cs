using Microsoft.AspNetCore.Mvc;
using BuilderAux.VOs;
using BuilderAux.DTO_s;

namespace BuilderAux.Repository.Usuarios
{
    public interface IUsuariosRepository
    {
        public Task<Dictionary<string, string>> GetAsync();
        public Task<Dictionary<string, string>> GetByEmailAsync(string email);
        public Task<UsuariosVO> PostAsync(UsuariosVO user);
        public Task PutAsync(string email, UsuariosVO user);
        public Task<bool> DeleteAsync(string email);
        public Task MudarSenha(string novaSenha, string email);
        public Task<string> Login(Login userLogin);
    }
}
