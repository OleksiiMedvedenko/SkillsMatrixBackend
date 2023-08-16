using Data.Repository.Interface;
using Models.CreateModels;
using Models.Status;
using Models.ViewModels;
using Services.ServiceAsync.Interface;

namespace Services.ServiceAsync
{
    public class EmpoyeeAsyncService : IEmployeeAsyncService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAreasAsyncService _areaRepository;
        private readonly IPositionAsyncService _positionAsyncService;
        private readonly IPermissionAsyncService _permissionAsyncService;

        public EmpoyeeAsyncService(IEmployeeRepository employeeRepository, IAreasAsyncService areaAsyncRepository, 
                                    IPositionAsyncService positionAsyncService, IPermissionAsyncService permissionAsyncService)
        {
            _employeeRepository = employeeRepository;
            _areaRepository = areaAsyncRepository;
            _positionAsyncService = positionAsyncService;
            _permissionAsyncService = permissionAsyncService;
        }


        public async Task<IEnumerable<EmployeeViewModel>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetEmployeesAsync();

            if (employees.ErrorExist)
            {
                throw new Exception(employees.ErrorMassage);
            }

            return employees.Result ?? throw new Exception("Error while getting data from table (Employee)");
        }
        public async Task<EmployeeViewModel> GetEmployeeAsync(int? employeeId)
        {
            if (employeeId == null)
            {
                { throw new ArgumentNullException(nameof(employeeId)); }
            }

            var employees = await GetEmployeesAsync();

            var employee = employees.SingleOrDefault(e => e.Employee?.EmployeeId == employeeId);

            var areas = await _areaRepository.GetEmployeeAreasAsync(employeeId);

            employee?.Areas?.AddRange(areas);

            return employee ?? throw new Exception("Error getting employee from database or employee does not exist!");
        }

        public async Task<bool> ActivateEmployeeAsync(int? employeeId)
        {
            if (employeeId == null)
            {
                { throw new ArgumentNullException(nameof(employeeId)); }
            }

            var result = await _employeeRepository.ActivateEmployeeAsync(employeeId);

            if (result.ErrorExist)
            {
                throw new Exception(result.ErrorMassage);
            }

            return result.Result.Equals(false)
                ? throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!")
                : result.Result;
        }

        public async Task<bool> CreateEmployeeAsync(EmployeeCreateModel? employee)
        {
            if (employee == null || employee?.FirstName == null) 
            { 
                throw new ArgumentNullException(nameof(employee)); 
            }

            var result = await _employeeRepository.CreateEmployeeAsync(employee);

            if (result.ErrorExist)
            {
                throw new Exception(result.ErrorMassage);
            }

            return result.Result.Equals(false)
                ? throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!")
                : result.Result;
        }

        public async Task<bool> DeactivateEmployeeAsync(int? employeeId)
        {
            if (employeeId == null)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }

            var result = await _employeeRepository.DeactivateEmployeeAsync(employeeId);

            if (result.ErrorExist)
            {
                throw new Exception(result.ErrorMassage);
            }

            return result.Result.Equals(false) 
                ? throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!")
                : result.Result;
        }

        public async Task<bool> UpdateEmployeeAsync(UpdateEmpoyeeModel? employee)
        {
            if(employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            //update employee positions and areas

            await _positionAsyncService.DeactivateAllEmployeePositions(employee?.Id);

            var employeeAreasBeforeUpdate = await _areaRepository.GetEmployeeAreasAsync(employee?.Id);

            var beforeAreas = employeeAreasBeforeUpdate.Select(x => x.AreaId).ToList();
            var actualAreas = employee?.AreasId;

            var commonElements = employee?.AreasId?.Intersect(beforeAreas);
            var differentElements = employee?.AreasId?.Except(beforeAreas);

#pragma warning disable CS8604 // Possible null reference argument.
            if (commonElements.Any())
            {
                foreach (var element in commonElements)
                {
                    await _positionAsyncService.ChangeStatusEmployeePositionAsync(element, employee?.Id, ActivationStatus.Active);

                }
            }
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
            // because the same position for all areas !!!!!!!!!
            if (differentElements.Any())
            {
                foreach (var element in differentElements)
                {
                    var positions = await _positionAsyncService.GetPositionsAsync();
                    var actualPositionInArea = positions.Where(x => x.AreaId == element).SingleOrDefault(x => x.Name == employee?.PositionName);
                    await _positionAsyncService.SetAnEmployeeNewPosition(actualPositionInArea?.PositionId, employee?.Id);
                }
            }
#pragma warning restore CS8604 // Possible null reference argument.

            //permissions 
            var permissionsResult = await _permissionAsyncService.UpdateEmployeePermissionStatusAsync(employee?.Id, ActivationStatus.Inactive);
            if (permissionsResult != false)
            {
                //check if some permission exist id db 
                var permission = await _permissionAsyncService.GetEmployeePermissonsAsync(employee?.Id, employee?.PermissionId, ActivationStatus.Inactive);

                if (permission.PermissionId == null)
                {
                    await _permissionAsyncService.SetNewPermissionToEmployeeAsync(employee?.Id, employee?.PermissionId);
                }
                else
                {
                    await _permissionAsyncService.UpdateEmployeePermissionStatusAsync(employee?.Id, ActivationStatus.Active, employee?.PermissionId);
                }
            }
            else {
                await _permissionAsyncService.SetNewPermissionToEmployeeAsync(employee?.Id, employee?.PermissionId);
            }
            
            //update data in table employee
            var result = await _employeeRepository.UpdateEmployeeAsync(employee);

            if (result.ErrorExist)
            {
                throw new Exception(result.ErrorMassage);
            }

            return result.Result.Equals(false)
               ? throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!")
               : result.Result;
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetEmployeeByDepartmentAsync(int? departmentId)
        {
            if (departmentId == null)
            {
                throw new ArgumentNullException(nameof(departmentId));
            }

            var employees = await GetEmployeesAsync();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return employees.Where(x => x.Department.DepartmentId.Equals(departmentId)) ?? throw new Exception("Error while getting data!");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}
