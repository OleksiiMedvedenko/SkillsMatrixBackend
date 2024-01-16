using Models.Model;
using Models.Status;
using Tools.DataService;

namespace Data.Repository.Interface
{
    public interface IPermissionRepository
    {
        Task<ExternalDataResultManager<IEnumerable<Permission>>> GetPermissionsAsync();
        Task<ExternalDataResultManager<bool>> CreatePermissionAsync(Permission? permission);
        /// <summary>
        /// if positionId == null => deactivate all employee permissions
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="activationStatus"></param>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        Task<ExternalDataResultManager<bool>> UpdateEmployeePermissionStatusAsync(int? employeeId, ActivationStatus activationStatus, int? permissionId = null);
        /// <summary>
        /// Get all employee permission from database active/inactive
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        Task<ExternalDataResultManager<Permission>> GetEmployeePermissonsAsync(int? employeeId, int? permissionId, ActivationStatus? status);
        Task<ExternalDataResultManager<bool>> SetNewPermissionToEmployeeAsync(int? employeeId, int? permissionId);
    }
}
