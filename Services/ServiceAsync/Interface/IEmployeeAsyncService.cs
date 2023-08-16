using Models.CreateModels;
using Models.Model;
using Models.ViewModels;

namespace Services.ServiceAsync.Interface
{
    public interface IEmployeeAsyncService
    {
        Task<IEnumerable<EmployeeViewModel>> GetEmployeesAsync();
        Task<IEnumerable<EmployeeViewModel>> GetEmployeeByDepartmentAsync(int? departmentId);

        Task<EmployeeViewModel> GetEmployeeAsync(int? employeeId);

        Task<bool> CreateEmployeeAsync(EmployeeCreateModel? employee);
        Task<bool> UpdateEmployeeAsync(UpdateEmpoyeeModel? employee);
        Task<bool> DeactivateEmployeeAsync(int? employeeId);
        Task<bool> ActivateEmployeeAsync(int? employeeId);
    }
}
