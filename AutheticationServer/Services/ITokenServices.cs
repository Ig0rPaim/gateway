using AutheticationServer.DTOs;
using AutheticationServer.Models;

namespace AutheticationServer.Services
{
    public interface ITokenServices
    {
        public Task<ResultCreate> CreateToken(UsuarioModel acesso, string code);
        public Task<ResultValidator> ValidateToken(string token, UsuarioModel acesso, string code);
        public bool sla();
    }
}
