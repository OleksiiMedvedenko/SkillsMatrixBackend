using Models.Model;

namespace Services.ServiceAsync.Interface
{
    public interface IDepartmentAsyncService
    {
        Task<Department> GetDepartmentAsync(int? departmentId, int? userId);
        Task<IEnumerable<Department>> GetDepartmentsAsync(int? userId);
        Task<bool> CreateDepartmentAsync(Department? department, int? userId);
    }
}
