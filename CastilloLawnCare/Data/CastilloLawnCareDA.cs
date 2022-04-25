using Microsoft.Data.SqlClient;
using System.Configuration;

namespace CastilloLawnCare.Data
{
    public class CastilloLawnCareDA
    {
        private IConfiguration configuration;
        public CastilloLawnCareDA()
        {
             configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }
        public SqlConnection CreateConnection()
        {
            
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message);
            }
            return sqlConnection;
        }
    }
}
