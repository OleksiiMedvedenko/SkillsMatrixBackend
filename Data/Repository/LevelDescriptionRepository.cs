using Data.Repository.Interface;
using Microsoft.Extensions.Configuration;
using Models.CreateModels;
using Models.Model;
using Tools.DataService;

namespace Data.Repository
{
    public class LevelDescriptionRepository : DatabaseProviderController, ILevelDescriptionRepository
    {
        public LevelDescriptionRepository(IConfiguration? configuration) : base(configuration) { }

        public async Task<ExternalDataResultManager<bool>> CreateLevelsDescriptionAsync(int? auditId)
        {
            var commandResult = false;

            var sqlQuery = @"INSERT INTO [AuditLevelDescription] (auditId, description, level) VALUES (@auditId, 'Opis', @level)";

            try
            {
                connection?.Open();
                for (int i = 0; i <= 2; i++)
                {
                    var command = CreateCommand(sqlQuery);
                    command.Parameters.AddWithValue("@auditId", auditId);
                    command.Parameters.AddWithValue("@level", i);

                    if (await command.ExecuteNonQueryAsync() > 0)
                    {
                        commandResult = true;
                    }
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

        public async Task<ExternalDataResultManager<bool>> UpdateLevelsDescriptionAsync(EditLevelDescription[] descriptions)
        {
            var commandResult = false;

            var sqlQuery = @"UPDATE [AuditLevelDescription] SET description = @description WHERE levelDescriptionId = @levelDescriptionId";

            try
            {
                connection?.Open();

                foreach (var data in descriptions)
                {
                    var command = CreateCommand(sqlQuery);
                    command.Parameters.AddWithValue("@description", data?.Description);
                    command.Parameters.AddWithValue("@levelDescriptionId", data?.LevelDescriptionId);

                    if (await command.ExecuteNonQueryAsync() > 0)
                    {
                        commandResult = true;
                    }
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

        public async Task<ExternalDataResultManager<IEnumerable<LevelDescription>>> GetAuditLevelsDescriptionAsync(int? auditId)
        {
            var descriptions = new List<LevelDescription>();

            var sqlQuery = @"SELECT levelDescriptionId, auditId, description, level FROM [CompetitiveMatrix].[dbo].[AuditLevelDescription] WHERE auditId = @auditId";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@auditId", auditId);

            try
            {
                connection?.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        var description = new LevelDescription(reader.GetFieldValue<int>(0), reader.GetFieldValue<int>(1), reader.GetFieldValue<int>(3), reader.GetFieldValue<string>(2));

                        descriptions.Add(description);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<LevelDescription>>(descriptions, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<IEnumerable<LevelDescription>>(descriptions);
        }

        public async Task<ExternalDataResultManager<IEnumerable<LevelDescription>>> GetDepartmentAuditLevelDescriptionAsync(int? department)
        {
            var descriptions = new List<LevelDescription>();

            var sqlQuery = @"SELECT ald.auditId, a.auditName, ald.level, ald.description FROM [dbo].[AuditLevelDescription] as ald 
                              LEFT JOIN Audit as a ON a.auditID = ald.auditId
                              INNER JOIN [dbo].[DepartamentArea] as da ON da.areaID = a.areaID
                              WHERE da.departamentID = @deparmentId";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@deparmentId", department);

            try
            {
                connection?.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        var description = new LevelDescription(reader.GetFieldValue<int>(0), reader.GetFieldValue<string>(1), reader.GetFieldValue<int>(2), reader.GetFieldValue<string>(3));

                        descriptions.Add(description);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<LevelDescription>>(descriptions, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<IEnumerable<LevelDescription>>(descriptions);
        }
    }
}
