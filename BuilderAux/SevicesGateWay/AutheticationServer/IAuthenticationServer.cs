using BuilderAux.DTO_s;

namespace BuilderAux.SevicesGateWay.AutheticationServer
{
    public interface IAuthenticationServer
    {
        public Task<ResultCreateToken> CreateToken(Authentication acesso, HttpContext context);
        public Task<ResultValidateToken> ValidateToken(Authentication acesso);
    }
}
