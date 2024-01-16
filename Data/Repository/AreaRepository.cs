using Data.Repository.Interface;
using Microsoft.Extensions.Configuration;
using Models.CreateModels;
using Models.Model;
using Tools.DataService;

namespace Data.Repository
{
    public class AreaRepository : DatabaseProviderController, IAreaRepository
    {
        public AreaRepository(IConfiguration? configuration) : base(configuration) { }

        public async Task<ExternalDataResultManager<bool>> CreateAreaAsync(AreaCreateModel? area)
        {
            var commandResult = false;

            var sqlQuery = @"INSERT Area ([areaName]) OUTPUT INSERTED.areaID, @departmentID 
                             INTO DepartamentArea([areaID], [departamentID]) 
                             VALUES (@areaName)";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@departmentID", area?.DepartmentId);
            command.Parameters.AddWithValue("@areaName", area?.AreaName);

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

        public async Task<ExternalDataResultManager<IEnumerable<Area>>> GetAreasAsync()
        {
            var areas = new List<Area>();

            var sqlQuery = @"SELECT a.areaID, a.areaName, d.departamentID, d.departamentName FROM DepartamentArea AS dA
	                        INNER JOIN Departament as d
	                        ON dA.departamentID = d.departamentID
	                        INNER JOIN Area as a 
	                        ON dA.areaID = a.areaID";

            var command = CreateCommand(sqlQuery);

            try
            {
                connection?.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        Area area = new(reader.GetFieldValue<int>(0), reader.GetFieldValue<string>(1),
                                             new Department(reader.GetFieldValue<int>(2), reader.GetFieldValue<string>(3)));

                        areas.Add(area);
                    }
                }

            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<Area>>(areas, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<IEnumerable<Area>>(areas);
        }

        public async Task<ExternalDataResultManager<IEnumerable<Area>>> GetEmployeeAreasAsync(int? employeeId)
        {
            var areas = new List<Area>();

            var sqlQuery = @"SELECT a.areaID, a.areaName, p.positionID, ps.isActive FROM [dbo].[Positions] as ps 
							INNER JOIN [dbo].[Position] as p ON p.positionID = ps.positionID
							INNER JOIN [dbo].[Area] as a ON a.areaID = p.areaID
							WHERE employeeID = @employeeId";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@employeeId", employeeId);

            try
            {
                connection?.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        Area area = new(reader.GetFieldValue<int>(0), reader.GetFieldValue<string>(1), null, reader.GetFieldValue<int>(2), reader.GetBoolean(3));

                        areas.Add(area);
                    }
                }

            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<Area>>(areas, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<IEnumerable<Area>>(areas);
        }
    }
}
