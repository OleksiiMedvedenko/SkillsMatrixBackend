using Data.Repository;
using Data.Repository.Interface;
using Models.Model;
using Services.ServiceAsync.Interface;

namespace Services.ServiceAsync
{
    public class DepartmentAsyncService : IDepartmentAsyncService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentAsyncService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<bool> CreateDepartmentAsync(Department? department)
        {
            if (department == null || department?.DepartmentId == null)
            {
                throw new ArgumentNullException(nameof(department));
            }

            var result = await _departmentRepository.CreateDepartmentAsync(department);

            if (result.ErrorExist)
            {
                throw new Exception(result.ErrorMassage);
            }

            return result.Result.Equals(false)
                ? throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!")
                : result.Result;
        }

        public async Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            var departments = await _departmentRepository.GetDepartmentsAsync();

            if (departments.ErrorExist)
            {
                throw new Exception(departments.ErrorMassage);
            }

            return departments.Result ?? throw new Exception("Error while getting data from table (Department)");
        }

        public async Task<Department> GetDepartmentAsync(int? departmentId)
        {
            if (departmentId == null)
            {
                throw new ArgumentNullException(nameof(departmentId));
            }

            var departments = await GetDepartmentsAsync();

            var department = departments.SingleOrDefault(d => d.DepartmentId == departmentId);

            return department ?? throw new Exception("Error getting employee from database or employee does not exist!");
        }
    }
}
