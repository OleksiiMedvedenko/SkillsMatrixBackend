using Models.CreateModels;
using Models.Model;
using Tools.DataService;

namespace Services.ServiceAsync.Interface
{
    public interface IAreasAsyncService
    {
        Task<IEnumerable<Area>> GetAreasAsync();
        Task<bool> CreateAreaAsync(AreaCreateModel? area);

        Task<IEnumerable<Area>> GetEmployeeAreasAsync(int? employeeId);
    }
}
