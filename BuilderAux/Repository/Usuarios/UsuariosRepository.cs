using BuilderAux.Data.ConnectionString;
using BuilderAux.Senhas;
using BuilderAux.Criptografia;
using BuilderAux.VOs;
using Microsoft.AspNetCore.Mvc;
using System.Data;
//using System.Data.SqlClients;
using System.Data.SqlClient;
using BuilderAux.SevicesGateWay.Mail;
using SendGrid.Helpers.Errors.Model;
using BuilderAux.Exceptions;

namespace BuilderAux.Repository.Usuarios
{
    public class UsuariosRepository : IUsuariosRepository
    {
        public async Task<bool> Delete(string email)
        {
            #region init
            SqlCommand cmd = new SqlCommand();
            string user;
            string connectionString = StringConnection.GetString();
            #endregion
            using ( SqlConnection cn = new SqlConnection(connectionString))
            {
                #region connection
                cmd.Connection = cn;
                //cmdFind.Connection = cn;

                cn.Open();
                SqlTransaction transaction = cn.BeginTransaction();
                cmd.Transaction = transaction;
                //cmdFind.Transaction = transaction;
                #endregion
                user = await FindEmail(email);
                if (user == null) throw new Exceptions.NotFoundException("Usuario não encontrado");
                try
                {
                    cmd.CommandText = "DELETE FROM Usuarios WHERE Id=@idUser;";
                    cmd.Parameters.Add("@idUser", SqlDbType.NVarChar).Value = user;
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                    return await Task.FromResult(true);
                }
                catch (SqlException er)
                {
                    transaction.Rollback();
                    string[] error = new string[3];
                    error[0] = er.Message;
                    error[1] = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    error[2] = DateTime.Now.ToString();
                    #region sendMail
                    _ = SendMail.SendGridMail(
                        "igorpaimdeoliveira@gmail.com",
                        "Igor",
                        "testemanipulacaoemail",
                        "Engenheiro Master",
                        "Erro ao deletar Usuário"
                        );
                    #endregion
                    throw er;
                }
                finally { cn.Close(); }
            }
        }

        public Task<IEnumerable<TResult>> Get<TResult>()
        {
            throw new NotImplementedException();
        }

        public Task<TResult> Get<TResult, T>(T id)
        {
            throw new NotImplementedException();
        }

        public async Task<UsuariosVO> Post(UsuariosVO user) // passivel de não retornar nada ou um bool
        {
            #region init
            SqlCommand cmd = new SqlCommand();
            SqlCommand cmdRole = new SqlCommand();
            string role = "";
            string connectionString = StringConnection.GetString();
            #endregion
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string retorno = await FindEmail(user.Email);
                if (!string.IsNullOrEmpty(retorno)) { throw new AlreadyExists("O usuario já existe"); }
                #region connection
                cmdRole.Connection = cn;
                cmd.Connection = cn;
                cn.Open();
                SqlTransaction transaction = cn.BeginTransaction();
                cmdRole.Transaction = transaction;
                cmd.Transaction = transaction;
                #endregion
                try
                {
                    cmdRole.Parameters.Add("@Cargo", SqlDbType.NChar, 10).Value = user.Role;
                    cmdRole.CommandText = "SELECT Id FROM RolesUsuarios WHERE Cargo = @Cargo;";
                    SqlDataReader idRole = cmdRole.ExecuteReader();
                    if (idRole.Read()) { role = idRole["Id"].ToString() ?? throw new ArgumentNullException(); }
                    idRole.Close();
                    #region criptografia
                    string senhaRetorno = CriptrografiaAndDescriptografia
                         .Criptografar(
                          GeradorDeSenhas.GerarSenha()
                        );
                    #endregion
                    cmd.CommandText = "INSERT INTO Usuarios (Id, Nome, Email, Senha, DataCadastro, Role)" +
                        "VALUES (@Id, @Nome, @Email, @Senha, @DataCadastro, @Role);";
                    #region Parametros
                    cmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Guid.NewGuid().ToString();
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = CriptrografiaAndDescriptografia.Criptografar(user.Email);
                    cmd.Parameters.Add("@Nome", SqlDbType.NVarChar).Value = user.Name;
                    cmd.Parameters.Add("@DataCadastro", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@Role", SqlDbType.NVarChar).Value = role;
                    cmd.Parameters.Add("@Senha", SqlDbType.VarBinary).Value = CriptografiaSenha.getInByteArray(senhaRetorno);
                    #endregion
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                    user.Senha = senhaRetorno;
                    #region sendMail
                    _ = SendMail.SendGridMail(
                        "igorpaimdeoliveira@gmail.com",
                        "Igor",
                        "igorpaimdeoliveira@gmail.com",
                        user.Name,
                        "Sua Senha",
                        $"Sua senha é {senhaRetorno}",
                        "google.com"
                        );
                    #endregion
                    return await Task.FromResult(user);
                }
                catch (SqlException er)
                {
                    transaction.Rollback();
                    string[] error = new string[3];
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

        private Task<string> FindEmail(string email)
        {
            #region init
            SqlCommand cmd = new SqlCommand();
            string user = "";
            string connectionString = StringConnection.GetString();
            #endregion
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                #region connection
                cmd.Connection = cn;
                cn.Open();
                SqlTransaction transaction = cn.BeginTransaction();
                cmd.Transaction = transaction;
                #endregion
                try
                {
                    cmd.CommandText = $"SELECT Id FROM Usuarios WHERE Email=@email;";
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = CriptrografiaAndDescriptografia.Criptografar(email);
                    SqlDataReader idUser = cmd.ExecuteReader();
                    if (idUser.Read()) { user = idUser["Id"].ToString() ?? throw new ArgumentNullException(); }
                    else { return Task.FromResult(string.Empty); }
                    idUser.Close();
                    return Task.FromResult(user);
                }
                catch (SqlException er)
                {
                    transaction.Rollback();
                    string[] error = new string[3];
                    error[0] = er.Message;
                    error[1] = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    error[2] = DateTime.Now.ToString();
                    #region sendMail
                    _ = SendMail.SendGridMail(
                        "igorpaimdeoliveira@gmail.com",
                        "Igor",
                        "testemanipulacaoemail",
                        "Engenheiro Master",
                        "Usuário não encontrado"
                        );
                    #endregion
                    throw er;
                }
                finally { cn.Close(); }
            }
        }
    }
}
