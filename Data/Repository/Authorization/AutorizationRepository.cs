using Data.Repository.Authorization.Interface;
using Microsoft.Extensions.Configuration;
using Models.Model;
using Models.ViewModels;
using Tools.DataService;

namespace Data.Repository.Authorization
{
    public class AutorizationRepository : DatabaseProviderController, IAutorizationRepository
    {
        public AutorizationRepository(IConfiguration? configuration) : base(configuration) { }

        public async Task<ExternalDataResultManager<EmployeeViewModel>> Authentication(string? login)
        {
            var data = new EmployeeViewModel();

            var sqlQuery = @$"SELECT e.employeeID, e.firstName, e.lastName, p.permissionID, p.permissionName, d.departamentID, d.departamentName, e.login FROM [dbo].[Employee] as e 
                              LEFT JOIN Permissions as ps ON ps.employeeID = e.employeeID
                              LEFT JOIN Permission as p ON p.permissionID = ps.permissionID
                              LEFT JOIN Departament as d ON d.departamentID = e.departamentID
                                WHERE e.login = @login AND ps.isActive = 1";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@login", login);

            try
            {
                connection?.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (await reader.ReadAsync())
                    {
                        EmployeeViewModel employee = new EmployeeViewModel(
                            reader.GetFieldValue<int>(0),
                            new Employee(reader.GetFieldValue<int>(0), reader.GetFieldValue<string>(1), reader.GetFieldValue<string>(2), true, null, reader.GetFieldValue<string>(7)),
                            null, null, null,
                            new Department(reader.GetFieldValue<int>(5), reader.GetFieldValue<string>(6)),
                            new Permission(reader.GetFieldValue<short>(3), reader.GetFieldValue<string>(4)));


                        data = employee;
                    }
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<EmployeeViewModel>(data, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<EmployeeViewModel>(data);
        }
    }
}
