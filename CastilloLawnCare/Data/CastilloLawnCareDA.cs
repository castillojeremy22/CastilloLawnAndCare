using Microsoft.Data.SqlClient;
using MySqlConnector;
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
        public MySqlConnection CreateConnection()
        {
            
            string connectionString = configuration.GetConnectionString("CastilloLawnCareConn");
            MySqlConnection sqlConnection = new MySqlConnection(connectionString);
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
