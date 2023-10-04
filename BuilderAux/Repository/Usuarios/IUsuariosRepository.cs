using Microsoft.AspNetCore.Mvc;
using BuilderAux.VOs;

namespace BuilderAux.Repository.Usuarios
{
    public interface IUsuariosRepository
    {
        public Task<IEnumerable<TResult>> Get<TResult>();
        public Task<TResult> Get<TResult, T>(T id);
        public Task<UsuariosVO> Post(UsuariosVO Value);
        public Task<TResult> Put<TResult, T, T1>(T id, [FromBody] T1 value);
        public Task<TResult> Delete<TResult, T>(T id);
    }
}
