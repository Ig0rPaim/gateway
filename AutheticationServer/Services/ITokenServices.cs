using AutheticationServer.Models;

namespace AutheticationServer.Services
{
    public interface ITokenServices
    {
        object CreateToken(UsuarioModel acesso);
        object ValidateToken(string token, UsuarioModel acesso);
    }
}
