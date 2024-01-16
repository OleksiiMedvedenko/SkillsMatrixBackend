using Data.Repository.Form.Interface;
using Microsoft.Extensions.Configuration;
using Models.CreateModels;
using Models.Model;
using Tools.DataService;

namespace Data.Repository.Form
{
    public class AuditDataRepository : DatabaseProviderController, IAuditDataRepository
    {
        public AuditDataRepository(IConfiguration? configuration) : base(configuration) { }

        public async Task<ExternalDataResultManager<int?>> SaveAuditDataAsync(CreateAuditDocumentModel? data)
        {
            int? saveId = null;

            var sqlQuery = @"INSERT INTO [dbo].[AuditDocument] (auditHistoryID, auditDocumentTemplateId, datetime, auditorId, Line) 
                                OUTPUT INSERTED.[auditDocumentId]
                                VALUES (@auditHistoryId, @auditDocumentTemplateId ,GETDATE(), @auditorId, @line)";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@auditHistoryId", data.AuditHistoryId);
            command.Parameters.AddWithValue("@auditDocumentTemplateId", data?.AuditTemplateId);
            command.Parameters.AddWithValue("@auditorId", data?.AuditorId);
            command.Parameters.AddWithValue("@line", data?.Line);

            try
            {
                connection?.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            saveId = reader.GetInt32(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<int?>(saveId, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<int?>(saveId);
        }

        public async Task<ExternalDataResultManager<bool>> SaveAuditResultsAsync(AuditDocumentValues[]? data)
        {
            var commandResult = false;

            var sqlQuery = @"INSERT INTO [dbo].[AuditDocumentValues] (auditDocumentId, questionId, value, comment)
                                VALUES (@auditDocumentId, @questionId, @value, @comment)";

            try
            {
                connection?.Open();

                foreach (var questionData in data)
                {
                    var command = CreateCommand(sqlQuery);
                    command.Parameters.AddWithValue("@auditDocumentId", questionData?.AuditDocumentId);
                    command.Parameters.AddWithValue("@questionId", questionData?.QuestionId);
                    command.Parameters.AddWithValue("@value", questionData?.Value);
                    command.Parameters.AddWithValue("@comment", ((object?)questionData?.Comment) ?? DBNull.Value); 

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
    }
}
