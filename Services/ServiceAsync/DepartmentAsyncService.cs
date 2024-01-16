using Data.LoggerRepository.Interface;
using Data.Repository.Interface;
using Models.Model;
using Services.ServiceAsync.Interface;

namespace Services.ServiceAsync
{
    public class DepartmentAsyncService : IDepartmentAsyncService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger _logger;

        public DepartmentAsyncService(IDepartmentRepository departmentRepository, ILogger logger)
        {
            _departmentRepository = departmentRepository;
            _logger = logger;
        }

        public async Task<bool> CreateDepartmentAsync(Department? department, int? userId)
        {
            if (department == null || department?.DepartmentId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie nowego działu", $"Parametr przekazany do metody ({nameof(CreateDepartmentAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(department));
            }

            var result = await _departmentRepository.CreateDepartmentAsync(department);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie nowego działu", $"Występuje błąd w metodzie: {nameof(CreateDepartmentAsync)}", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false))
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie nowego działu", $"Występuje błąd w metodzie: {nameof(CreateDepartmentAsync)}", "Nie udało się utworzyć nowy dział"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }

            return result.Result;
        }

        public async Task<IEnumerable<Department>> GetDepartmentsAsync(int? userId)
        {
            var departments = await _departmentRepository.GetDepartmentsAsync();

            if (departments.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pozyskiwanie danych o działach", $"Występuje błąd w metodzie: {nameof(GetDepartmentsAsync)}", departments.ErrorMassage));
                throw new Exception(departments.ErrorMassage);
            }

            if (departments?.Result?.Count() == 0)
            {
                //await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pozyskiwanie danych o działach", $"Występuje błąd w metodzie: {nameof(GetDepartmentsAsync)}", "Nie udało się pobrać danych z bazy/brak danych"));
                throw new Exception("Error while getting data from table (Department)");
            }

            return departments.Result;
        }

        public async Task<Department> GetDepartmentAsync(int? departmentId, int? userId)
        {
            if (departmentId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskanie danych o dziale", $"Parametr przekazany do metody ({nameof(GetDepartmentAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(departmentId));
            }

            var departments = await GetDepartmentsAsync(userId);

            var department = departments.SingleOrDefault(d => d.DepartmentId == departmentId);

            if (department == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskanie danych o dziale", $"Występuje błąd w metodzie: {nameof(GetDepartmentAsync)}", "Nie udało się pobrać danych o dziale/brak danych"));
                throw new Exception("Error getting employee from database or employee does not exist!");
            }

            return department;
        }
    }
}
