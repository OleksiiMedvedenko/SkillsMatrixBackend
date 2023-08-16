using Models.CreateModels;
using Models.Model;
using Models.Status;
using Tools.DataService;

namespace Services.ServiceAsync.Interface
{
    public interface IPositionAsyncService
    {
        Task<IEnumerable<Position>> GetPositionsAsync();
        Task<IEnumerable<Position>> GetPositionsByAreaAsync(int? areaId);

        Task<bool> CreatePositionAsync(PositionCreateModel? position);
        Task<bool> ChangeStatusEmployeePositionAsync(int? areaId, int? employeeId, ActivationStatus? positionStatus);
        Task<bool> DeactivateAllEmployeePositions(int? employeeId);
        Task<bool> SetAnEmployeeNewPosition(int? positionId, int? employeeId);
    }
}
