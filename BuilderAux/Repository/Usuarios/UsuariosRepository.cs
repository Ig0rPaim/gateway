using BuilderAux.Data.ConnectionString;
using BuilderAux.Senhas;
using BuilderAux.Criptografia;
using BuilderAux.VOs;
using Microsoft.AspNetCore.Mvc;
using System.Data;
//using System.Data.SqlClients;
using System.Data.SqlClient;
using BuilderAux.SevicesGateWay.Mail;

namespace BuilderAux.Repository.Usuarios
{
    public class UsuariosRepository : IUsuariosRepository
    {
        //private SqlCommand cmd = new SqlCommand();
        //private SqlConnection conn = new SqlConnection();

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

        public Task<UsuariosVO> Post(UsuariosVO user)
        {
            SqlCommand cmd = new SqlCommand();
            SqlCommand cmdRole = new SqlCommand();
            string connectionString = StringConnection.GetString();
            //cmd.Parameters.Clear();
            //cmd.Parameters.Clear();
            using(SqlConnection cn = new SqlConnection(connectionString))
            {
                #region conecção
                cmd.Connection = cn;
                cmdRole.Connection = cn;
                cn.Open();
                SqlTransaction transaction = cn.BeginTransaction();
                cmd.Transaction = transaction;
                #endregion
                try
                {
                    #region buscandoIdRole
                    cmdRole.Parameters.Add("@Cargo", SqlDbType.NChar, 10).Value = user.Role;
                    cmdRole.CommandText = "SELECT Id FROM RolesUsuarios WHERE Cargo = @Cargo;";
                    SqlDataReader idRole = cmdRole.ExecuteReader();
                    if (idRole == null) { throw new NullReferenceException(); }
                    string role = idRole.ToString();
                    #endregion
                    #region criptografia
                    string senhaRetorno = CriptrografiaAndDescriptografia
                         .Criptografar(
                          GeradorDeSenhas.GerarSenha()
                        );
                    #endregion
                    #region Parametros

                    cmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Guid.NewGuid().ToString();
                    cmd.Parameters.Add("@Email", SqlDbType.VarBinary).Value = user.Email;
                    cmd.Parameters.Add("@Nome", SqlDbType.NVarChar).Value = user.Name;
                    cmd.Parameters.Add("@DataCadastro", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@Role", SqlDbType.NVarChar).Value = role;
                    cmd.Parameters.Add("@Senha", SqlDbType.VarBinary).Value = CriptografiaSenha.getInByteArray(senhaRetorno);
                    #endregion
                    cmd.CommandText = "INSERT INTO Usuarios (Id, Nome, Email, Senha, DataCadastro, Role)" +
                        "VALUES (@Id, @Nome, @Email, @Senha, @DataCadastro, @Role);";
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                    user.Senha = senhaRetorno;
                    return Task.FromResult(user);
                }
                catch (SqlException er)
                {
                    transaction.Rollback();
                    string[] error =  new string[3];
                    error[0] = er.Message;
                    error[1] = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    error[2] = DateTime.Now.ToString();
                    #region sendMail
                    _ = SendMail.SendGridMail(
                        "igorpaimdeoliveira@gmail.com",
                        "Igor",
                        "testemanipulacaoemail",
                        "Engenheiro Master",
                        "Erro ao inserir novo Usuário"
                        );
                    #endregion
                    throw er;

                }
                finally { cn.Close(); }
            }
        }

        public Task<TResult> Put<TResult, T, T1>(T id, [FromBody] T1 value)
        {
            throw new NotImplementedException();
        }
    }
}
