using Microsoft.Data.SqlClient;

namespace DockerWebApi.Repository
{
    public static class HelloRepository
    {
        private static string ConnectionString = string.Empty;

        static HelloRepository()
        {
            ConnectionString = Config.Configuration.GetConnectionString("Default");
        }

        internal static string Connect()
        {
            string result = string.Empty;

            using (var sqlConnection = new SqlConnection(ConnectionString))
            using (var cmd = sqlConnection.CreateCommand())
            {
                sqlConnection.Open();

                cmd.CommandText = "select GETDATE()";

                result = cmd.ExecuteScalar().ToString();

                sqlConnection.Close();
            }

            return result;
        }
    }
}
