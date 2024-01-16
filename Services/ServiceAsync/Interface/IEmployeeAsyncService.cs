using Models.CreateModels;
using Models.Model;
using Models.ViewModels;

namespace Services.ServiceAsync.Interface
{
    public interface IEmployeeAsyncService
    {
        Task<IEnumerable<EmployeeViewModel>> GetEmployeesAsync(int? userId);
        Task<IEnumerable<EmployeeViewModel>> GetEmployeeByDepartmentAsync(int? departmentId, int? userId);

        Task<EmployeeViewModel> GetEmployeeAsync(int? employeeId, int? userId);

        Task<bool> CreateEmployeeAsync(EmployeeCreateModel? employee, int? userId);
        Task<bool> UpdateEmployeeAsync(UpdateEmpoyeeModel? employee, int? userId);
        Task<bool> DeactivateEmployeeAsync(int? employeeId, int? userId);
        Task<bool> ActivateEmployeeAsync(int? employeeId, int? userId);
    }
}
