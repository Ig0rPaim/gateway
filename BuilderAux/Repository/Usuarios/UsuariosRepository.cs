using BuilderAux.Data.ConnectionString;
using BuilderAux.Senhas;
using BuilderAux.Criptografia;
using BuilderAux.VOs;
using System.Data;
using System.Data.SqlClient;
using BuilderAux.SevicesGateWay.Mail;
using BuilderAux.Exceptions;
using BuilderAux.DTO_s;
using BuilderAux.SevicesGateWay.Token;

//Em produção

namespace BuilderAux.Repository.Usuarios
{
    public class UsuariosRepository : IUsuariosRepository
    {
        public async Task<bool> DeleteAsync(string email)
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
                cn.Open();
                SqlTransaction transaction = cn.BeginTransaction();
                cmd.Transaction = transaction;
                #endregion
                user = await FindEmail(email);
                if (user == "") throw new Exceptions.NotFoundException("Usuario não encontrado");
                try
                {
                    cmd.CommandText = @"DELETE FROM Usuarios
                                      WHERE Id=@idUser;";
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

        public async Task<Dictionary<string, string>> GetAsync()
        {
            #region init
            SqlCommand cmd = new SqlCommand();
            Dictionary<string, string> user = new Dictionary<string, string>();
            string connectionString = StringConnection.GetString();
            #endregion
            using(SqlConnection cn = new SqlConnection(connectionString))
            {
                #region connection
                cmd.Connection = cn;
                cn.Open();
                #endregion
                try
                {
                    cmd.CommandText = @"SELECT Nome, Cargo 
                                        FROM Usuarios 
                                        INNER JOIN RolesUsuarios 
                                        ON Usuarios.Role = RolesUsuarios.Id";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        user.Add(reader["Nome"].ToString() ?? throw new ArgumentNullException(),
                            reader["Cargo"].ToString() ?? throw new ArgumentNullException());
                    }
                    return await Task.FromResult(user);
                }
                catch (SqlException er)
                {
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
                        "Erro ao buscar Usuários"
                        );
                    #endregion
                    throw er;
                }
                finally { cn.Close(); }
            }
        }

        public async Task<Dictionary<string,string>> GetByEmailAsync(string email)
        {
            #region init
            SqlCommand cmd = new SqlCommand();
            Dictionary<string, string> user = new Dictionary<string, string>();
            string connectionString = StringConnection.GetString();
            string userId;
            userId = await FindEmail(email);
            if (userId == null) throw new Exceptions.NotFoundException("Usuario não encontrado");
            #endregion
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                #region
                cmd.Connection = cn;
                cn.Open();
                #endregion
                try
                {
                    cmd.CommandText = @"SELECT Usuarios.Nome, RolesUsuarios.Cargo
                                        FROM Usuarios
                                        INNER JOIN RolesUsuarios ON Usuarios.Role = RolesUsuarios.Id
                                        WHERE Usuarios.Id = @IdUser";
                    cmd.Parameters.Add("@IdUser", SqlDbType.NVarChar).Value = userId;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        user.Add(reader["Nome"].ToString() ?? throw new ArgumentNullException(),
                            reader["Cargo"].ToString() ?? throw new ArgumentNullException()); ;
                    }
                    return user;
                }
                catch (SqlException er)
                {
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
                finally { cn.Close() ; }
            }
        }

        public async Task<UsuariosVO> PostAsync(UsuariosVO user) // passivel de não retornar nada ou um bool
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
                    #region GetRole
                    cmdRole.Parameters.Add("@Cargo", SqlDbType.NChar, 10).Value = user.Role;
                    cmdRole.CommandText = @"SELECT Id 
                                          FROM RolesUsuarios
                                          WHERE Cargo = @Cargo;";
                    SqlDataReader idRole = cmdRole.ExecuteReader();
                    if (idRole.Read()) { role = idRole["Id"].ToString() ?? throw new ArgumentNullException(); }
                    idRole.Close();
                    #endregion
                    #region criptografia
                    string senhaRetorno = CriptrografiaAndDescriptografia
                         .Criptografar(
                          GeradorDeSenhas.GerarSenha()
                        );
                    #endregion
                    cmd.CommandText = @"INSERT INTO Usuarios (Id, Nome, Email, Senha, DataCadastro, Role, Telefone)
                                      VALUES (@Id, @Nome, @Email, @Senha, @DataCadastro, @Role, @Telefone);";
                    #region Parametros
                    cmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Guid.NewGuid().ToString();
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = CriptrografiaAndDescriptografia.Criptografar(user.Email);
                    cmd.Parameters.Add("@Nome", SqlDbType.NVarChar).Value = user.Name;
                    cmd.Parameters.Add("@Telefone", SqlDbType.NVarChar).Value = user.Telefone;
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

        public async Task PutAsync(string email, UsuariosVO user)
        {
            #region init
            SqlCommand cmd = new SqlCommand();
            SqlCommand cmdRole = new SqlCommand();
            string connectionString = StringConnection.GetString();
            string userId;
            string role = "";
            #endregion
            userId = await FindEmail(email);
            if (userId == null) throw new Exceptions.NotFoundException("Usuario não encontrado");
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                #region connection
                cmd.Connection = cn;
                cmdRole.Connection = cn;
                cn.Open();
                SqlTransaction transaction = cn.BeginTransaction();
                cmd.Transaction = transaction;
                cmdRole.Transaction = transaction;
                #endregion
                try
                {
                    #region GetRole
                    cmdRole.Parameters.Add("@Cargo", SqlDbType.NChar).Value = user.Role;
                    cmdRole.CommandText = @"SELECT Id 
                                          FROM RolesUsuarios
                                          WHERE Cargo = @Cargo;";
                    SqlDataReader idRole = cmdRole.ExecuteReader();
                    if (idRole.Read()) { role = idRole["Id"].ToString() ?? throw new ArgumentNullException(); }
                    idRole.Close();
                    #endregion
                    #region criptografia
                    string senhaRetorno = CriptrografiaAndDescriptografia
                         .Criptografar(
                          GeradorDeSenhas.GerarSenha()
                        );
                    #endregion

                    cmd.CommandText = @"UPDATE Usuarios
                                      SET Nome=@Nome, Email=@Email, Senha=@Senha, DataAtualizacao=@Atualizacao, Role=@role
                                      WHERE Usuarios.Id=@UserId";
                    #region parametros
                    cmd.Parameters.Add("@Nome", SqlDbType.NVarChar).Value = user.Name;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = CriptrografiaAndDescriptografia.Criptografar(user.Email);
                    cmd.Parameters.Add("@senha", SqlDbType.VarBinary).Value = CriptografiaSenha.getInByteArray(senhaRetorno);
                    cmd.Parameters.Add("@Atualizacao", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = userId;
                    cmd.Parameters.Add("@role", SqlDbType.NVarChar).Value = role;
                    #endregion
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
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
                finally { cn.Close() ; }
            }
        }

        private async Task<string> FindEmail(string email)
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
                    cmd.CommandText = @"SELECT Id FROM Usuarios WHERE Email=@email;";
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = CriptrografiaAndDescriptografia.Criptografar(email);
                    SqlDataReader idUser = cmd.ExecuteReader();
                    if (idUser.Read()) { user = idUser["Id"].ToString() ?? throw new ArgumentNullException(); }
                    else { return await Task.FromResult(string.Empty); }
                    idUser.Close();
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
                        "Usuário não encontrado"
                        );
                    #endregion
                    throw er;
                }
                finally { cn.Close(); }
            }
        }

        public async Task MudarSenha(string novaSenha, string email)
        {
            #region init
            SqlCommand cmd = new SqlCommand();
            string user;
            string connectionString = StringConnection.GetString();
            #endregion
            using(SqlConnection cn = new SqlConnection(connectionString))
            {
                #region connection
                cmd.Connection = cn;
                cn.Open();
                SqlTransaction transaction = cn.BeginTransaction();
                cmd.Transaction = transaction;
                #endregion
                user = await FindEmail(email);
                if (user == null) throw new Exceptions.NotFoundException("Usuario não encontrado");
                try
                {
                    cmd.CommandText = @"UPDATE Usuarios
                                       SET Senha=@NovaSenha
                                       WHERE Usuarios.Id = @userId";
                    #region parametros
                    cmd.Parameters.Add("@NovaSenha", SqlDbType.VarBinary).Value = CriptografiaSenha.getInByteArray(novaSenha);
                    cmd.Parameters.Add("@userId", SqlDbType.NVarChar).Value = user;
                    #endregion
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
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

        public async Task<string> Login(Login userLogin)
        {
            #region init
            SqlCommand cmd = new SqlCommand();
            string user;
            byte[] passUser = CriptografiaSenha.getInByteArray(userLogin.Senha ?? string.Empty);
            byte[] pass;
            TokenService generateToken = new TokenService();
            string connectionString = StringConnection.GetString();
            #endregion
            using(SqlConnection cn = new SqlConnection(connectionString))
            {
                #region connection
                cmd.Connection = cn;
                cn.Open();
                SqlTransaction transaction = cn.BeginTransaction();
                cmd.Transaction = transaction;
                #endregion
                user = await FindEmail(userLogin.email);
                if (user == "") throw new Exceptions.NotFoundException("Usuario não encontrado");
                try
                {
                    cmd.CommandText = @"SELECT Nome, Telefone, senha, Cargo 
                                      FROM Usuarios 
                                      INNER JOIN RolesUsuarios 
                                      ON Usuarios.Role = RolesUsuarios.Id
                                      WHERE Usuarios.Id = @IdUser;
                                      ";
                    cmd.Parameters.Add("@IdUser", SqlDbType.NVarChar).Value = user;
                    SqlDataReader reader = cmd.ExecuteReader();
                    _ = reader.Read() ? pass = (byte[])reader["Senha"]
                        ?? throw new ArgumentNullException() 
                        : throw new Exception();
                    UsuariosVO userToken = new UsuariosVO(
                        reader["Nome"].ToString() ?? string.Empty,
                        userLogin.email,
                        reader["Telefone"].ToString() ?? string.Empty,
                        reader["Cargo"].ToString() ?? string.Empty
                    );
                    if (pass.SequenceEqual(passUser)) { return generateToken.Generate(userToken); }
                    return string.Empty;
                }
                catch (SqlException er)
                {
                    throw er;
                }
                finally { cn.Close(); }
            }
        }
    }
}
