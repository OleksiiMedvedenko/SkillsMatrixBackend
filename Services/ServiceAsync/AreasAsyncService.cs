using Data.LoggerRepository.Interface;
using Data.Repository.Interface;
using Models.CreateModels;
using Models.Model;
using Services.ServiceAsync.Interface;

namespace Services.ServiceAsync
{
    public class AreasAsyncService : IAreasAsyncService
    {
        private readonly IAreaRepository _areaRepository;
        private readonly ILogger _logger;

        public AreasAsyncService(IAreaRepository areaRepository, ILogger logger)
        {
            _areaRepository = areaRepository;
            _logger = logger;
        }

        public async Task<bool> CreateAreaAsync(AreaCreateModel? area, int? userId)
        {
            if (area == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie obszaru", $"Parametr przekazany do metody ({nameof(CreateAreaAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(area));
            }

            var result = await _areaRepository.CreateAreaAsync(area);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie obszaru", $"Obszar należy do działu: {area?.DepartmentId} ID", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false))
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie obszaru", $"Występuje błąd w metodzie ({nameof(CreateAreaAsync)})", "Obszaru nie został utworzony"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }

            return result.Result;
        }


        public async Task<IEnumerable<Area>> GetAreasAsync(int? userId)
        {
            var areas = await _areaRepository.GetAreasAsync();

            if (areas.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pobieranie danych obszarów", $"Występuje błąd w metodzie: {nameof(GetAreasAsync)}", areas.ErrorMassage));
                throw new Exception(areas.ErrorMassage);
            }

            if (areas?.Result?.Count() == 0)
            {
                //await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pobieranie danych obszarów", $"Występuje błąd w metodzie: {nameof(GetAreasAsync)}", "Nie udało się pobrać danych z bazy/brak danych"));
                throw new Exception("Error while getting data from table (Areas)");
            }

            return areas.Result;
        }

        public async Task<IEnumerable<Area>> GetEmployeeAreasAsync(int? employeeId, int? userId)
        {
            if (employeeId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pozyskiwanie danych pracownika w obszarach, które go dotyczą", $"Parametr przekazany do metody ({nameof(GetEmployeeAreasAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(employeeId));
            }

            var areas = await _areaRepository.GetEmployeeAreasAsync(employeeId);

            if (areas.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pozyskiwanie danych pracownika w obszarach, które go dotyczą", $"Występuje błąd w metodzie: {nameof(GetEmployeeAreasAsync)}", areas.ErrorMassage));
                throw new Exception(areas.ErrorMassage);
            }


            if (areas?.Result?.Count() == 0)
            {
                //await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pozyskiwanie danych pracownika w obszarach, które go dotyczą", $"Występuje błąd w metodzie: {nameof(GetEmployeeAreasAsync)}", "Nie udało się pobrać danych z bazy/brak danych"));
                throw new Exception("Error while getting data from table (Areas)");
            }
            return areas.Result;
        }
    }
}
