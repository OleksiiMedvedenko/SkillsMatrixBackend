using Data.LoggerRepository.Interface;
using Microsoft.Extensions.Configuration;
using Models.AppModel;
using Tools.DataService;

namespace Data.LoggerRepository
{
    public class Logger : DatabaseProviderController, ILogger
    {
        public Logger(IConfiguration? configuration) : base(configuration) { }

        public async Task<ExternalDataResultManager> DeclareErrorAsync(LoggerModel loggerModel)
        {
            var sqlQuery = @"INSERT INTO [CompetitiveMatrix].[dbo].[ErrorApplicationLogger] (userId, title, message, error, date) VALUES (@userId, @title, @message, @error, GETDATE())";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@userId", ((object?)loggerModel?.UserId) ?? DBNull.Value);     
            command.Parameters.AddWithValue("@title", ((object?)loggerModel?.Title) ?? DBNull.Value);
            command.Parameters.AddWithValue("@message", ((object?)loggerModel?.Message) ?? DBNull.Value);
            command.Parameters.AddWithValue("@error", ((object?)loggerModel?.Error) ?? DBNull.Value);

            try
            {
                connection?.Open();

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager(ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager();
        }
    }
}
