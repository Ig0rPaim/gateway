using BuilderAux.Data.ConnectionString;
using BuilderAux.Senhas;
using BuilderAux.VOs;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BuilderAux.Repository.Usuarios
{
    public class UsuariosRepository : IUsuariosRepository
    {
        public Task<TResult> Delete<TResult, T>(T id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResult>> Get<TResult>()
        {
            throw new NotImplementedException();
        }

        public Task<TResult> Get<TResult, T>(T id)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> Post<TResult>(UsuariosVO Value)
        {
            string connectionString = StringConnection.GetString();
            string queryString =
                "INSERT INTO Usuarios (Id, Nome, Email, Senha, DataCadastro, Role)" +
                "VALUES (@Id, @Nome, @Email, @Senha, @DataCadastro, @Role);";

            string Nome = Value.Name;
            string Email = Value.Email;
            string Id = Guid.NewGuid().ToString();
            string DataCadastro = DateTime.Now.ToString();
            string SenhaRetorno = GeradorDeSenhas.GerarSenha();

            throw new NotImplementedException();

        }

        public Task<TResult> Put<TResult, T, T1>(T id, [FromBody] T1 value)
        {
            throw new NotImplementedException();
        }
    }
}
