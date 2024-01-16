using Data.LoggerRepository.Interface;
using Data.Repository.Interface;
using Models.Model;
using Models.Status;
using Services.ServiceAsync.Interface;

namespace Services.ServiceAsync
{
    public class PermissionAsyncService : IPermissionAsyncService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly ILogger _logger;

        public PermissionAsyncService(IPermissionRepository permissionRepository, ILogger logger)
        {
            _permissionRepository = permissionRepository;
            _logger = logger;
        }

        public async Task<bool> CreatePermissionAsync(Permission? permission, int? userId)
        {
            if (permission == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie uprawnienia", $"Parametr przekazany do metody ({nameof(CreatePermissionAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(permission));
            }

            var result = await _permissionRepository.CreatePermissionAsync(permission);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie uprawnienia", $"Występuje błąd w metodzie: {nameof(CreatePermissionAsync)}", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false)) 
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie uprawnienia", $"Występuje błąd w metodzie: {nameof(CreatePermissionAsync)}", "Nie udało się utworzyć nowego uprawnienia"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }

            return result.Result;
        }

        public async Task<IEnumerable<Permission>> GetPermissionsAsync(int? userId)
        {
            var permissions = await _permissionRepository.GetPermissionsAsync();

            if (permissions.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskiwanie danych o uprawnieniach", $"Występuje błąd w metodzie: {nameof(GetPermissionsAsync)}", permissions.ErrorMassage));
                throw new Exception(permissions.ErrorMassage);
            }

            return permissions.Result ?? throw new Exception("Error while getting data from table (Permission)");
        }

        public async Task<bool> SetNewPermissionToEmployeeAsync(int? employeeId, int? permissionId, int? userId)
        {
            if (employeeId == null || permissionId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Ustawienie nowych uprawnień dla pracownika", $"Parametr przekazany do metody ({nameof(SetNewPermissionToEmployeeAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(employeeId) + nameof(permissionId));
            }

            var result = await _permissionRepository.SetNewPermissionToEmployeeAsync(employeeId, permissionId);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Ustawienie nowych uprawnień dla pracownika", $"Występuje błąd w metodzie: {nameof(SetNewPermissionToEmployeeAsync)}", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false))
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Ustawienie nowych uprawnień dla pracownika", $"Występuje błąd w metodzie: {nameof(SetNewPermissionToEmployeeAsync)}", $"Nie udało się ustawić uprawnienia: {permissionId}id"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }

            return result.Result;
        }

        public async Task<bool> UpdateEmployeePermissionStatusAsync(int? employeeId, ActivationStatus activationStatus, int? permissionId = null, int? userId = null)
        {
            if (employeeId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Edycja uprawnień pracownika", $"Parametr przekazany do metody ({nameof(UpdateEmployeePermissionStatusAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(employeeId));
            }

            var result = await _permissionRepository.UpdateEmployeePermissionStatusAsync(employeeId, activationStatus, permissionId);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Edycja uprawnień pracownika", $"Występuje błąd w metodzie: {nameof(UpdateEmployeePermissionStatusAsync)}", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            return result.Result;
        }

        public async Task<Permission> GetEmployeePermissonsAsync(int? employeeId, int? permissionId, ActivationStatus? status, int? userId)
        {
            var permissions = await _permissionRepository.GetEmployeePermissonsAsync(employeeId, permissionId, status);

            if (permissions.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskiwanie danych o uprawnieniach pracownika", $"Występuje błąd w metodzie: {nameof(GetEmployeePermissonsAsync)}", permissions.ErrorMassage));
                throw new Exception(permissions.ErrorMassage);
            }

            return permissions.Result ?? throw new Exception("Error while getting data from table (Permissions)");
        }
    }
}
