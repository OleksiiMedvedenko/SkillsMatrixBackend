using Models.CreateModels;
using Models.Model;
using Models.Status;
using Tools.DataService;

namespace Services.ServiceAsync.Interface
{
    public interface IPositionAsyncService
    {
        Task<IEnumerable<Position>> GetPositionsAsync(int? userId);
        Task<IEnumerable<Position>> GetPositionsByAreaAsync(int? areaId, int? userId);

        Task<bool> CreatePositionAsync(PositionCreateModel? position, int? userId);
        Task<bool> ChangeStatusEmployeePositionAsync(int? positionId, int? employeeId, ActivationStatus? positionStatus, int? userId);
        Task<bool> DeactivateAllEmployeePositions(int? employeeId, int? userId);
        Task<bool> SetAnEmployeeNewPosition(int? positionId, int? employeeId, int? userId);
    }
}
