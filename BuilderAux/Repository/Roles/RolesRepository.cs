using BuilderAux.Data.ConnectionString;
using System.Data;
using System.Data.SqlClient;

namespace BuilderAux.Repository.Roles
{
    public class RolesRepository : IRolesRepository
    {
        private SqlCommand cmd = new SqlCommand();
        public void Post(string Cargo)
        {
            string connectionString = StringConnection.GetString();
            cmd.Parameters.Clear();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cmd.Connection = cn;
                cn.Open();
                SqlTransaction transaction = cn.BeginTransaction();
                cmd.Transaction = transaction;

                try
                {
                    cmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Guid.NewGuid().ToString();
                    cmd.Parameters.Add("@Cargo", SqlDbType.NChar).Value = Cargo;

                    cmd.CommandText = "INSERT INTO RolesUsuarios (Id, Cargo)" +
                        "VALUES (@Id, @Cargo);";
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (SqlException er)
                {
                    transaction.Rollback();
                    throw er;
                }
                finally { cn.Close(); }
            }
        }
    }
}
