using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.DataService
{
    public class DatabaseProviderController
    {
        protected readonly string? connectionString;
        protected readonly SqlConnection? connection;
        private readonly IConfiguration? _configuration;

        protected DatabaseProviderController(IConfiguration? configuration, string connectionToDB = "DevConnection")
        {
            _configuration = configuration;

            string? connectionString = _configuration?.GetConnectionString(connectionToDB);
            connection = new SqlConnection(connectionString);
        }

        protected DatabaseProviderController(string connectionStringKey)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringKey].ToString();
            connection = new SqlConnection(connectionString);
        }

        protected SqlCommand CreateCommand(string command, bool isProcedure = false)
        {
            var output = new SqlCommand(command, connection);

            if (isProcedure)
            {
                output.CommandType = CommandType.StoredProcedure;
            }

            return output;
        }
    }
}
