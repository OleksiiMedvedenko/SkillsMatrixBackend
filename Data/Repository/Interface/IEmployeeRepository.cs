using Models.CreateModels;
using Models.Model;
using Models.ViewModels;
using Tools.DataService;

namespace Data.Repository.Interface
{
    public interface IEmployeeRepository
    {
        Task<ExternalDataResultManager<IEnumerable<EmployeeViewModel>>> GetEmployeesAsync();

        Task<ExternalDataResultManager<int?>> CreateEmployeeAsync(EmployeeCreateModel? employee);
        Task<ExternalDataResultManager<bool>> UpdateEmployeeAsync(UpdateEmpoyeeModel? employee);
        Task<ExternalDataResultManager<bool>> DeactivateEmployeeAsync(int? employeeId);
        Task<ExternalDataResultManager<bool>> ActivateEmployeeAsync(int? employeeId);
    }
}
