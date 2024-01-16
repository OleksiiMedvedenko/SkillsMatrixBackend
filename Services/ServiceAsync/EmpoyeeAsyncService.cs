using Data.LoggerRepository.Interface;
using Data.Repository.Interface;
using Models.CreateModels;
using Models.Model;
using Models.Status;
using Models.ViewModels;
using Services.ServiceAsync.Interface;
using System.Xml.Linq;

namespace Services.ServiceAsync
{
    public class EmpoyeeAsyncService : IEmployeeAsyncService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAreasAsyncService _areaRepository;
        private readonly IPositionAsyncService _positionAsyncService;
        private readonly IPermissionAsyncService _permissionAsyncService;
        private readonly ILogger _logger;

        public EmpoyeeAsyncService(IEmployeeRepository employeeRepository, IAreasAsyncService areaAsyncRepository, 
                                    IPositionAsyncService positionAsyncService, IPermissionAsyncService permissionAsyncService, ILogger logger)
        {
            _employeeRepository = employeeRepository;
            _areaRepository = areaAsyncRepository;
            _positionAsyncService = positionAsyncService;
            _permissionAsyncService = permissionAsyncService;
            _logger = logger;
        }


        public async Task<IEnumerable<EmployeeViewModel>> GetEmployeesAsync(int? userId)
        {
            var employees = await _employeeRepository.GetEmployeesAsync();

            if (employees.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pozyskiwanie danych o pracownikach", $"Występuje błąd w metodzie: {nameof(GetEmployeesAsync)}", employees.ErrorMassage));
                throw new Exception(employees.ErrorMassage);
            }

            if (employees?.Result?.Count() == 0)
            {
                //await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pozyskiwanie danych o pracownikach", $"Występuje błąd w metodzie: {nameof(GetEmployeesAsync)}", "brak danych"));
                throw new Exception("Error while getting data from table (Employee)");
            }

            return employees.Result;
        }
        public async Task<EmployeeViewModel> GetEmployeeAsync(int? employeeId, int? userId)
        {
            if (employeeId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskanie danych o pracowniku", $"Parametr przekazany do metody ({nameof(GetEmployeeAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(employeeId));
            }

            var employees = await GetEmployeesAsync(userId);

            var employee = employees.SingleOrDefault(e => e.Employee?.EmployeeId == employeeId);

            if (employee == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskanie danych o pracowniku", $"Występuje błąd w metodzie: {nameof(GetEmployeeAsync)}", "Nie udało się pobrać danych o audycie/brak danych"));
                throw new Exception("Error getting employee from database or employee does not exist!");
            }

            var areas = await _areaRepository.GetEmployeeAreasAsync(employeeId, userId);

            employee?.Areas?.AddRange(areas.Where(x => x.IsActive == true));

            return employee;
        }

        public async Task<bool> CreateEmployeeAsync(EmployeeCreateModel? employee, int? userId)
        {
            if (employee == null || employee?.FirstName == null) 
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie pracownika", $"Parametr przekazany do metody ({nameof(CreateEmployeeAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(employee)); 
            }

            var result = await _employeeRepository.CreateEmployeeAsync(employee);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie pracownika", $"Występuje błąd w metodzie: {nameof(CreateEmployeeAsync)}", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie pracownika", $"Występuje błąd w metodzie: {nameof(CreateEmployeeAsync)}", "Nie udało się utworzyć nowego pracownika"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }

            #region set employee position and area 

            var positions = await _positionAsyncService.GetPositionsAsync(userId);
            var position = positions.FirstOrDefault(x => x.PositionId == employee?.PositionId); // now get name of position which i send from front end
            var employeePositions = positions.Where(x => x.Name == position?.Name);

            
#pragma warning disable CS8629 // Nullable value type may be null.
            var commonElements = employeePositions.Where(p => (bool)(employee?.AreasId?.Contains(p.AreaId))).ToList();
#pragma warning restore CS8629 // Nullable value type may be null.

            if (commonElements.Any())
            {
                foreach (var item in commonElements)
                {
                    await _positionAsyncService?.SetAnEmployeeNewPosition(item?.PositionId, result?.Result, userId);
                }
            }
#endregion

            return true;
        }

        public async Task<bool> ActivateEmployeeAsync(int? employeeId, int? userId)
        {
            if (employeeId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Aktywizacja pracownika", $"Parametr przekazany do metody ({nameof(ActivateEmployeeAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(employeeId));
            }

            var result = await _employeeRepository.ActivateEmployeeAsync(employeeId);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Aktywizacja pracownika", $"Występuje błąd w metodzie: {nameof(ActivateEmployeeAsync)}", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false))
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, $"Aktywizacja pracownika id:{employeeId}", $"Występuje błąd w metodzie: {nameof(ActivateEmployeeAsync)}", "Nie udalo się zrobić update w bazie"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }

            return result.Result;
        }

        public async Task<bool> DeactivateEmployeeAsync(int? employeeId, int? userId)
        {
            if (employeeId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Dezaktywacja pracownika", $"Parametr przekazany do metody ({nameof(DeactivateEmployeeAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(employeeId));
            }

            var result = await _employeeRepository.DeactivateEmployeeAsync(employeeId);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Dezaktywacja pracownika", $"Występuje błąd w metodzie: {nameof(DeactivateEmployeeAsync)}", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false))
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, $"Dezaktywacja pracownika id:{employeeId}", $"Występuje błąd w metodzie: {nameof(DeactivateEmployeeAsync)}", "Nie udalo się zrobić update w bazie"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }

            return result.Result;
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetEmployeeByDepartmentAsync(int? departmentId, int? userId)
        {
            if (departmentId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, $"Pobierania pracowników zatrudnionych w dziale: {departmentId} id", $"Parametr przekazany do metody ({nameof(GetEmployeeByDepartmentAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(departmentId));
            }

            var employees = await GetEmployeesAsync(userId);

            var result = employees.Where(x => x.Department.DepartmentId.Equals(departmentId));

            if (result.Count() == 0) // change to Count() == 0 ?
            {
                //await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pobierania pracowników zatrudnionych w dziale", $"Metoda: {nameof(GetEmployeeByDepartmentAsync)}", "Brak danych"));
                throw new Exception("Error while getting data!");
            }

            return result;
        }

        public async Task<bool> UpdateEmployeeAsync(UpdateEmpoyeeModel? employee, int? userId)
        {
            if (employee == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, $"Edycja pracownika: {employee?.Id} id", $"Parametr przekazany do metody ({nameof(UpdateEmployeeAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(employee));
            }


            //check actual employee positions
            var actualEmployeePositionsInAreas = await _areaRepository.GetEmployeeAreasAsync(employee?.Id, userId);

            #region now check new position which we get from front 
            var positions = await _positionAsyncService.GetPositionsAsync(userId);
            var position = positions.FirstOrDefault(x => x.PositionId == employee?.PositionId); // now get name of position which i send from front end
            var employeePositions = positions.Where(x => x.Name == position?.Name);
            #endregion

            var checkOnExistedPositionInDB = actualEmployeePositionsInAreas.FirstOrDefault(x => x.PositionId == employee.PositionId);
            //if employee don`t have this position early we deactivate all positions 
            if (checkOnExistedPositionInDB == null)
            {
                //now deactivate all employee positions 
                await _positionAsyncService.DeactivateAllEmployeePositions(employee?.Id, userId);

                #region SET NEW POSITION INTO DB 
#pragma warning disable CS8629 // Nullable value type may be null.
                var commonElements = employeePositions.Where(p => (bool)(employee?.AreasId?.Contains(p.AreaId))).ToList();
#pragma warning restore CS8629 // Nullable value type may be null.
                if (employeePositions.Any())
                {
                    foreach (var item in commonElements)
                    {
                        await _positionAsyncService?.SetAnEmployeeNewPosition(item?.PositionId, employee?.Id, userId);
                    }
                }
                #endregion
            }
            else
            {
                //now deactivate all employee positions 
                await _positionAsyncService.DeactivateAllEmployeePositions(employee?.Id, userId);

#pragma warning disable CS8629 // Nullable value type may be null.
                var commonElements = employeePositions.Where(p => (bool)(employee?.AreasId?.Contains(p.AreaId))).ToList();
#pragma warning restore CS8629 // Nullable value type may be null.
                foreach (var item in commonElements)
                {
                    await _positionAsyncService.ChangeStatusEmployeePositionAsync(item?.PositionId, employee?.Id, ActivationStatus.Active, userId);
                }
            }

            //permissions 
            if (employee?.PermissionId != null)
            {
                var permissionsResult = await _permissionAsyncService.UpdateEmployeePermissionStatusAsync(employee?.Id, ActivationStatus.Inactive);
                if (permissionsResult != false)
                {
                    //check if some permission exist id db 
                    var permission = await _permissionAsyncService.GetEmployeePermissonsAsync(employee?.Id, employee?.PermissionId, ActivationStatus.Inactive, userId);

                    if (permission.PermissionId == null)
                    {
                        await _permissionAsyncService.SetNewPermissionToEmployeeAsync(employee?.Id, employee?.PermissionId, userId);
                    }
                    else
                    {
                        await _permissionAsyncService.UpdateEmployeePermissionStatusAsync(employee?.Id, ActivationStatus.Active, employee?.PermissionId);
                    }
                }
                else
                {
                    await _permissionAsyncService.SetNewPermissionToEmployeeAsync(employee?.Id, employee?.PermissionId, userId);
                }
            }

            //update data in table employee
            var result = await _employeeRepository.UpdateEmployeeAsync(employee);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, $"Edycja pracownika: {employee?.Id} id", $"Występuje błąd w metodzie: {nameof(UpdateEmployeeAsync)}", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false))
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, $"Edycja pracownika: {employee?.Id} id", $"Występuje błąd w metodzie: {nameof(UpdateEmployeeAsync)}", "Nie udalo się zrobić update w bazie"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }

            return result.Result;

        }

//        public async Task<bool> UpdateEmployeeAsync(UpdateEmpoyeeModel? employee, int? userId) // is not working - remake , dont forget add logger  !!!!!!!!!!!
//        {
//            if(employee == null)
//            {
//                throw new ArgumentNullException(nameof(employee));
//            }

//            //update employee positions and areas

//            await _positionAsyncService.DeactivateAllEmployeePositions(employee?.Id, userId);

//            var employeeAreasBeforeUpdate = await _areaRepository.GetEmployeeAreasAsync(employee?.Id, userId);

//            var beforeAreas = employeeAreasBeforeUpdate.Select(x => x.AreaId).ToList();
//            var actualAreas = employee?.AreasId;

//            var commonElements = employee?.AreasId?.Intersect(beforeAreas);
//            var differentElements = employee?.AreasId?.Except(beforeAreas);

//#pragma warning disable CS8604 // Possible null reference argument.
//            if (commonElements.Any())
//            {
//                foreach (var element in commonElements)
//                {
//                    await _positionAsyncService.ChangeStatusEmployeePositionAsync(element, employee?.Id, ActivationStatus.Active, userId);

//                }
//            }
//#pragma warning restore CS8604 // Possible null reference argument.
//#pragma warning disable CS8604 // Possible null reference argument.
//            // because the same position for all areas !!!!!!!!!
//            if (differentElements.Any())
//            {
//                foreach (var element in differentElements)
//                {
//                    var positions = await _positionAsyncService.GetPositionsAsync(userId);
//                    var actualPositionInArea = positions.Where(x => x.AreaId == element).SingleOrDefault(x => x.Name == employee?.PositionName);
//                    await _positionAsyncService.SetAnEmployeeNewPosition(actualPositionInArea?.PositionId, employee?.Id, userId);
//                }
//            }
//#pragma warning restore CS8604 // Possible null reference argument.

//            //permissions 
//            if (employee?.PermissionId != null)
//            {
//                var permissionsResult = await _permissionAsyncService.UpdateEmployeePermissionStatusAsync(employee?.Id, ActivationStatus.Inactive);
//                if (permissionsResult != false)
//                {
//                    //check if some permission exist id db 
//                    var permission = await _permissionAsyncService.GetEmployeePermissonsAsync(employee?.Id, employee?.PermissionId, ActivationStatus.Inactive, userId);

//                    if (permission.PermissionId == null)
//                    {
//                        await _permissionAsyncService.SetNewPermissionToEmployeeAsync(employee?.Id, employee?.PermissionId, userId);
//                    }
//                    else
//                    {
//                        await _permissionAsyncService.UpdateEmployeePermissionStatusAsync(employee?.Id, ActivationStatus.Active, employee?.PermissionId);
//                    }
//                }
//                else
//                {
//                    await _permissionAsyncService.SetNewPermissionToEmployeeAsync(employee?.Id, employee?.PermissionId, userId);
//                }
//            }
            
//            //update data in table employee
//            var result = await _employeeRepository.UpdateEmployeeAsync(employee);

//            if (result.ErrorExist)
//            {
//                throw new Exception(result.ErrorMassage);
//            }

//            return result.Result.Equals(false)
//               ? throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!")
//               : result.Result;
//        }
    }
}
