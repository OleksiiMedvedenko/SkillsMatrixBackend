using Data.Repository.Interface;
using Microsoft.Extensions.Configuration;
using Models.Model;
using Tools.DataService;

namespace Data.Repository
{
    public class DepartmentRepository : DatabaseProviderController, IDepartmentRepository
    {
        public DepartmentRepository(IConfiguration? configuration) : base(configuration) { }
         
        public async Task<ExternalDataResultManager<bool>> CreateDepartmentAsync(Department? department)
        {
            bool commandResult = false;

            var sqlQuery = @"INSERT INTO Departament(departamentName) VALUES (@departmentName)";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@departmentName", department?.Name);

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

        public async Task<ExternalDataResultManager<IEnumerable<Department>>> GetDepartmentsAsync()
        {
            var departments = new List<Department>();

            var sqlQuery = @"SELECT [departamentID],[departamentName]
                               FROM [CompetitiveMatrix].[dbo].[Departament]";

            var command = CreateCommand(sqlQuery);

            try
            {
                connection?.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        Department department = new(reader.GetFieldValue<int>(0), reader.GetFieldValue<string>(1));

                        departments.Add(department);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<Department>>(departments, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<IEnumerable<Department>>(departments);
        }
    }
}
