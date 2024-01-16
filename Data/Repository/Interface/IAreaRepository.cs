using Models.CreateModels;
using Models.Model;
using Tools.DataService;

namespace Data.Repository.Interface
{
    public interface IAreaRepository
    {
        Task<ExternalDataResultManager<IEnumerable<Area>>> GetAreasAsync();
        Task<ExternalDataResultManager<bool>> CreateAreaAsync(AreaCreateModel? area);

        Task<ExternalDataResultManager<IEnumerable<Area>>> GetEmployeeAreasAsync(int? employeeId);
    }
}
