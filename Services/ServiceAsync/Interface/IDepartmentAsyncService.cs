using Models.Model;

namespace Services.ServiceAsync.Interface
{
    public interface IDepartmentAsyncService
    {
        Task<Department> GetDepartmentAsync(int? departmentId);
        Task<IEnumerable<Department>> GetDepartmentsAsync();
        Task<bool> CreateDepartmentAsync(Department? department);
    }
}
