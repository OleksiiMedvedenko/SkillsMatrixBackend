using Models.CreateModels;
using Models.Model;
using Models.Status;
using Tools.DataService;

namespace Data.Repository.Interface
{
    public interface IPositionRepositry
    {
        Task<ExternalDataResultManager<IEnumerable<Position>>> GetPositionsAsync();

        Task<ExternalDataResultManager<bool>> CreatePositionAsync(PositionCreateModel? position);

        /// <summary>
        /// in application - (in view deactivate area)
        /// </summary>
        Task<ExternalDataResultManager<bool>> ChangeStatusEmployeePositionAsync(int? areaId, int? employeeId, ActivationStatus? status);
        Task<ExternalDataResultManager<bool>> DeactivateAllEmployeePositions(int? employeeId);

        Task<ExternalDataResultManager<bool>> SetAnEmployeeNewPosition(int? positionId ,int? employeeId);
    }
}
