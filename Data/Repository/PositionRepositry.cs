using Data.Repository.Interface;
using Microsoft.Extensions.Configuration;
using Models.CreateModels;
using Models.Model;
using Models.Status;
using Tools.DataService;

namespace Data.Repository
{
    public class PositionRepositry : DatabaseProviderController, IPositionRepositry
    {
        public PositionRepositry(IConfiguration? configuration) : base(configuration)
        {
        }

        public async Task<ExternalDataResultManager<bool>> CreatePositionAsync(PositionCreateModel? position)
        {
            bool commandResult = false;

            var sqlQuery = @"INSERT INTO Position(positionName, areaID) VALUES (@name, @areaId)";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@name", position?.PositionName);
            command.Parameters.AddWithValue("@areaId", position?.AreaId);

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

        public async Task<ExternalDataResultManager<bool>> ChangeStatusEmployeePositionAsync(int? positionId, int? employeeId, ActivationStatus? status)
        {
            bool commandResult = false;

            var sqlQuery = @"UPDATE [CompetitiveMatrix].[dbo].[Positions] SET isActive = @status WHERE positionID = @posId AND employeeID = @employeeId";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@status", (int?)status);
            command.Parameters.AddWithValue("@posId", positionId);
            command.Parameters.AddWithValue("@employeeId", employeeId);

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

        public async Task<ExternalDataResultManager<IEnumerable<Position>>> GetPositionsAsync()
        {
            var positions = new List<Position>();

            var sqlQuery = @"SELECT positionID, positionName, areaID FROM [dbo].[Position]";

            var command = CreateCommand(sqlQuery);

            try
            {
                connection?.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        var position = new Position(reader.GetFieldValue<int>(0),
                                                    reader.GetFieldValue<string>(1),
                                                    reader.GetFieldValue<int>(2));

                        positions.Add(position);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<Position>>(positions, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<IEnumerable<Position>>(positions);
        }

        public async Task<ExternalDataResultManager<bool>> DeactivateAllEmployeePositions(int? employeeId)
        {
            bool commandResult = false;

            var sqlQuery = @"UPDATE [dbo].[Positions] set isActive = 0 WHERE employeeID = @employeeId";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@employeeId", employeeId);

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

        public async Task<ExternalDataResultManager<bool>> SetAnEmployeeNewPosition(int? positionId, int? employeeId)
        {
            bool commandResult = false;

            var sqlQuery = @"INSERT [dbo].[Positions] (positionID, employeeID, isActive) VALUES(@positionId ,@employeeId , 1)";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@positionId", positionId);
            command.Parameters.AddWithValue("@employeeId", employeeId);

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
