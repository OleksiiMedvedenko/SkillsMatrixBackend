using Models.CreateModels;
using Models.Model;
using Tools.DataService;

namespace Services.ServiceAsync.Interface
{
    public interface IAreasAsyncService
    {
        Task<IEnumerable<Area>> GetAreasAsync(int? userId);
        Task<bool> CreateAreaAsync(AreaCreateModel? area, int? userId);

        Task<IEnumerable<Area>> GetEmployeeAreasAsync(int? employeeId, int? userId);
    }
}
