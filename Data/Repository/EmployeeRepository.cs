using Data.Repository.Interface;
using Microsoft.Extensions.Configuration;
using Models.CreateModels;
using Models.Model;
using Models.ViewModels;
using Tools.DataService;

namespace Data.Repository
{
    public class EmployeeRepository : DatabaseProviderController, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration? configuration) : base(configuration) { }

        public async Task<ExternalDataResultManager<bool>> ActivateEmployeeAsync(int? employeeId)
        {
            bool commandResult = false;

            var sqlquery = @"UPDATE Employee Set isActive = 1 WHERE employeeID = @id";

            var command = CreateCommand(sqlquery);
            command.Parameters.AddWithValue("@id", employeeId);

            try
            {
                connection?.Open();

                if (await command.ExecuteNonQueryAsync() > 0)
                {
                    commandResult = true;
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<bool>(commandResult, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<bool>(commandResult);
        }

        public async Task<ExternalDataResultManager<bool>> DeactivateEmployeeAsync(int? employeeId)
        {
            bool commandResult = false;

            var sqlquery = @"UPDATE Employee Set isActive = 0 WHERE employeeID = @id";

            var command = CreateCommand(sqlquery);
            command.Parameters.AddWithValue("@id", employeeId);

            try
            {
                connection?.Open();

                if (await command.ExecuteNonQueryAsync() > 0)
                {
                    commandResult = true;
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<bool>(commandResult, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<bool>(commandResult);
        }

        public async Task<ExternalDataResultManager<int?>> CreateEmployeeAsync(EmployeeCreateModel? employee)
        {
            int? createdEmployeId = null;

            var sqlQuery = @"INSERT INTO Employee(firstName, lastName, supervisorID, departamentID, isActive, login)
                            OUTPUT INSERTED.employeeID
                            VALUES(@fName, @lName, @supervisorId, @departmentId, 1, @login)";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@fName", employee?.FirstName);
            command.Parameters.AddWithValue("@lName", employee?.LastName);
            command.Parameters.AddWithValue("@supervisorId", ((object?)employee?.SupervisorId) ?? DBNull.Value);
            command.Parameters.AddWithValue("@departmentId", employee?.DepartmentId);
            command.Parameters.AddWithValue("@login", employee?.Login);

            try
            {
                connection?.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            createdEmployeId = reader.GetInt32(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<int?> (createdEmployeId, ex.Message, ex);
            }
            finally 
            { 
                connection?.Close();
            }

            return new ExternalDataResultManager<int?>(createdEmployeId);
        }


        public async Task<ExternalDataResultManager<IEnumerable<EmployeeViewModel>>> GetEmployeesAsync()
        {
            var employees = new List<EmployeeViewModel>();

            var sqlQuery = @"SELECT e.employeeID, e.firstName, e.lastName, e.isActive, s.employeeID, s.firstName, s.lastName, p.positionName, d.departamentID, d.departamentName, e.login, MAX(p.positionID) as positionID, MAX(permission.permissionID) as permissionID, MAX(permission.permissionName) as permissionName
                            FROM [dbo].[Employee] as e  
                            LEFT JOIN Positions as ps ON ps.employeeID = e.employeeID 
                            LEFT JOIN Position as p ON ps.positionID = p.positionID
                            LEFT JOIN Departament as d ON e.departamentID = d.departamentID
                            LEFT JOIN [dbo].[Employee] as s ON s.employeeID = e.supervisorID
                            LEFT JOIN [dbo].[Permissions] as permissions ON permissions.employeeID = e.employeeID AND permissions.isActive = 1
                            LEFT JOIN [dbo].[Permission] as permission ON permission.permissionID = permissions.permissionID
                            GROUP BY e.employeeID, e.firstName, e.lastName, e.isActive, s.employeeID, s.firstName, s.lastName, p.positionName, d.departamentID, d.departamentName, e.login, ps.isActive
                            HAVING ps.isActive = 1";

            var command = CreateCommand(sqlQuery);

            try
            {
                connection?.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        var employee = new EmployeeViewModel(
                            id: reader.GetFieldValue<int>(0),
                            new Employee(reader.GetFieldValue<int>(0), reader.GetFieldValue<string>(1), reader.GetFieldValue<string>(2), reader.GetFieldValue<bool>(3), reader.GetFieldValue<int>(8), reader.IsDBNull(10) ? null : reader.GetFieldValue<string>(10)),
                            new Supervisor(reader.IsDBNull(4) ? null : reader.GetFieldValue<int>(4), reader.IsDBNull(5) ? null : reader.GetFieldValue<string>(5), reader.IsDBNull(6) ? null : reader.GetFieldValue<string>(6)),
                            new Position(reader.IsDBNull(11) ? null : reader.GetFieldValue<int>(11), reader.IsDBNull(7) ? null : reader.GetFieldValue<string>(7)),
                            new List<Area>(),
                            new Department(reader.GetFieldValue<int>(8), reader.GetFieldValue<string>(9)),
                            new Permission(reader.IsDBNull(12) ? null : reader.GetFieldValue<short>(12), reader.IsDBNull(13) ? null : reader.GetFieldValue<string>(13)));

                        employees.Add(employee);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<EmployeeViewModel>>(employees, ex.Message, ex);
            }
            finally
            {
                connection?.Close(); 
            }

            return new ExternalDataResultManager<IEnumerable<EmployeeViewModel>>(employees);
        }

        public async Task<ExternalDataResultManager<bool>> UpdateEmployeeAsync(UpdateEmpoyeeModel? employee)
        {
            bool commandResult = false;

            var sqlQuery = @"UPDATE [dbo].[Employee] SET firstName = @firstName, lastName = @lastName, supervisorID = @supervisorId, departamentID = @departmentId, login = @login WHERE employeeID = @employeeId";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@firstName", employee?.FirstName);
            command.Parameters.AddWithValue("@lastName", employee?.LastName);
            command.Parameters.AddWithValue("@supervisorId", ((object?)employee?.SupervisorId) ?? DBNull.Value);
            command.Parameters.AddWithValue("@departmentId", employee?.DepartmentId);
            command.Parameters.AddWithValue("@login", ((object?)employee?.Login) ?? DBNull.Value);
            command.Parameters.AddWithValue("@employeeId", employee?.Id);
            try
            {
                connection?.Open();

                if (await command.ExecuteNonQueryAsync() > 0)
                {
                    commandResult = true;
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<bool>(commandResult, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<bool>(commandResult);
        }
    }
}
