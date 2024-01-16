using Data.Repository.Interface;
using Microsoft.Extensions.Configuration;
using Models.Model;
using Models.Status;
using System.Data.SqlClient;
using Tools.DataService;

namespace Data.Repository
{
    public class PermissionRepository : DatabaseProviderController, IPermissionRepository
    {
        public PermissionRepository(IConfiguration? configuration) : base(configuration) { }

        public async Task<ExternalDataResultManager<bool>> CreatePermissionAsync(Permission? permission)
        {
            var commandResult = false;

            var sqlQuery = @"INSERT INTO [dbo].[Permission] (permissionName) VALUES(@name)";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@name", permission?.Name);

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

        public async Task<ExternalDataResultManager<IEnumerable<Permission>>> GetPermissionsAsync()
        {
            var permissions = new List<Permission>();

            var sqlQuery = @"SELECT permissionID, permissionName, description FROM  [dbo].[Permission]";

            var command = CreateCommand(sqlQuery);

            try
            {
                connection?.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        Permission permission = new(reader.GetFieldValue<short>(0), reader.GetFieldValue<string>(1));

                        permissions.Add(permission);
                    }
                }

            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<IEnumerable<Permission>>(permissions, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<IEnumerable<Permission>>(permissions);
        }

        public async Task<ExternalDataResultManager<bool>> SetNewPermissionToEmployeeAsync(int? employeeId, int? permissionId)
        {
            var commandResult = false;

            var sqlQuery = @"INSERT INTO [dbo].[Permissions] (permissionID, employeeID, isActive) VALUES (@permissionID, @employeeId, 1)";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@permissionID", permissionId);
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

        public async Task<ExternalDataResultManager<bool>> UpdateEmployeePermissionStatusAsync(int? employeeId, ActivationStatus activationStatus, int? permissionId = null)
        {
            bool commandResult = false;
            string sqlQuery = string.Empty;

            SqlCommand command;

            if (permissionId == null)
            {
                sqlQuery = @"UPDATE [dbo].[Permissions] SET isActive = 0 WHERE employeeID = @employeeId";
                command = CreateCommand(sqlQuery);
                command.Parameters.AddWithValue("@employeeId", employeeId);
            }
            else
            {
                sqlQuery = @"UPDATE [dbo].[Permissions] SET isActive = @status WHERE employeeID = @employeeId AND permissionID = @permissionID";
                command = CreateCommand(sqlQuery);
                command.Parameters.AddWithValue("@status", (int)activationStatus);
                command.Parameters.AddWithValue("@employeeId", employeeId);
                command.Parameters.AddWithValue("@permissionID", permissionId);
            }


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

        public async Task<ExternalDataResultManager<Permission>> GetEmployeePermissonsAsync(int? employeeId, int? permissionId, ActivationStatus? status)
        {
            var permission = new Permission();

            var sqlQuery = @"SELECT * FROM [dbo].[Permissions] WHERE employeeID = @employeeID AND permissionID = @permissionID AND isActive = @active";

            var command = CreateCommand(sqlQuery);
            command.Parameters.AddWithValue("@employeeID", employeeId);
            command.Parameters.AddWithValue("@permissionID", permissionId);
            command.Parameters.AddWithValue("@active", (int?)status);

            try
            {
                connection?.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        Permission _permission = new Permission(reader.GetFieldValue<short>(0), null);

                        permission = _permission;
                    }
                }

            }
            catch (Exception ex)
            {
                return new ExternalDataResultManager<Permission>(permission, ex.Message, ex);
            }
            finally
            {
                connection?.Close();
            }

            return new ExternalDataResultManager<Permission>(permission);
        }
    }
}
