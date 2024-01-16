using Models.Model;
using Models.Status;
using Tools.DataService;

namespace Services.ServiceAsync.Interface
{
    public interface IPermissionAsyncService
    {
        Task<IEnumerable<Permission>> GetPermissionsAsync(int? userId);
        Task<bool> CreatePermissionAsync(Permission? permission, int? userId);

        /// <summary>
        /// if positionId == null => deactivate all employee permissions
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="activationStatus"></param>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        Task<bool> UpdateEmployeePermissionStatusAsync(int? employeeId, ActivationStatus activationStatus, int? permissionId = null, int? userId = null);
        /// <summary>
        /// Get all employee permission from database active/inactive
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        Task<Permission> GetEmployeePermissonsAsync(int? employeeId, int? permissionId, ActivationStatus? status, int? userId);
        Task<bool> SetNewPermissionToEmployeeAsync(int? employeeId, int? permissionId, int? userId);
    }
}
