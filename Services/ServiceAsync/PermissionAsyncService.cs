using Data.Repository;
using Data.Repository.Interface;
using Models.Model;
using Models.Status;
using Services.ServiceAsync.Interface;

namespace Services.ServiceAsync
{
    public class PermissionAsyncService : IPermissionAsyncService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionAsyncService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<bool> CreatePermissionAsync(Permission? permission)
        {
            if (permission == null)
            {
                throw new ArgumentNullException(nameof(permission));
            }

            var result = await _permissionRepository.CreatePermissionAsync(permission);

            if (result.ErrorExist)
            {
                throw new Exception(result.ErrorMassage);
            }

            return result.Result.Equals(false)
                ? throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!")
                : result.Result;
        }

        public async Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            var permissions = await _permissionRepository.GetPermissionsAsync();

            if (permissions.ErrorExist)
            {
                throw new Exception(permissions.ErrorMassage);
            }

            return permissions.Result ?? throw new Exception("Error while getting data from table (Permission)");
        }

        public async Task<bool> SetNewPermissionToEmployeeAsync(int? employeeId, int? permissionId)
        {
            if (employeeId == null || permissionId == null)
            {
                throw new ArgumentNullException(nameof(employeeId) + nameof(permissionId));
            }

            var result = await _permissionRepository.SetNewPermissionToEmployeeAsync(employeeId, permissionId);

            if (result.ErrorExist)
            {
                throw new Exception(result.ErrorMassage);
            }

            return result.Result.Equals(false)
                ? throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!")
                : result.Result;
        }

        public async Task<bool> UpdateEmployeePermissionStatusAsync(int? employeeId, ActivationStatus activationStatus, int? permissionId = null)
        {
            if (employeeId == null)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }

            var result = await _permissionRepository.UpdateEmployeePermissionStatusAsync(employeeId, activationStatus, permissionId);

            if (result.ErrorExist)
            {
                throw new Exception(result.ErrorMassage);
            }

            return result.Result;
        }

        public async Task<Permission> GetEmployeePermissonsAsync(int? employeeId, int? permissionId, ActivationStatus? status)
        {
            var permissions = await _permissionRepository.GetEmployeePermissonsAsync(employeeId, permissionId, status);

            if (permissions.ErrorExist)
            {
                throw new Exception(permissions.ErrorMassage);
            }

            return permissions.Result ?? throw new Exception("Error while getting data from table (Permissions)");
        }
    }
}
