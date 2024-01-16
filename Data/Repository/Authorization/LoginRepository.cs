using Data.Repository.Authorization.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.DataService;

namespace Data.Repository.Authorization
{
    public class LoginRepository : DatabaseProviderController, ILoginRepository
    {
        public LoginRepository(IConfiguration? configuration) : base(configuration)
        {

        }

        public async Task<ExternalDataResultManager<int?>> Login(string? login, string? password)
        {
            int? procedureResult = -1;

            string sqlQuery = @"SELECT s.employeeId FROM [dbo].[Security] as s INNER JOIN [dbo].[Employee] as e ON e.employeeID = s.employeeId 
                                    WHERE s.login LIKE @login AND password LIKE @password";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@password", password);

            try
            {
                connection?.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        try
                        {
                            procedureResult = reader.GetFieldValue<int>(0);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<int?>(procedureResult, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<int?>(procedureResult);
        }
    }
}
