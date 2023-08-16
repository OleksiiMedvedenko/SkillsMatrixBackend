using Models.Model;
using Tools.DataService;

namespace Data.Repository.Interface
{
    public interface IDepartmentRepository
    {
        Task<ExternalDataResultManager<IEnumerable<Department>>> GetDepartmentsAsync();
        Task<ExternalDataResultManager<bool>> CreateDepartmentAsync(Department? department);

    }
}
