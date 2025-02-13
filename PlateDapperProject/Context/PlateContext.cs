using Microsoft.Data.SqlClient;
using System.Data;

namespace PlateDapperProject.Context
{
    public class PlateContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public PlateContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("connectionkey");
        }
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
