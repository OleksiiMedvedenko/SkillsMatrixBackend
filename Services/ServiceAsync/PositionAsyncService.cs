using Data.LoggerRepository.Interface;
using Data.Repository.Interface;
using Models.CreateModels;
using Models.Model;
using Models.Status;
using Services.ServiceAsync.Interface;

namespace Services.ServiceAsync
{
    public class PositionAsyncService : IPositionAsyncService
    {
        private readonly IPositionRepositry _positionRepositry;
        private readonly ILogger _logger;

        public PositionAsyncService(IPositionRepositry positionRepositry, ILogger logger)
        {
            _positionRepositry = positionRepositry;
            _logger = logger;
        }

        public async Task<bool> CreatePositionAsync(PositionCreateModel? position, int? userId)
        {
            if (position == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie stanowiska", $"Parametr przekazany do metody ({nameof(CreatePositionAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(position));
            }

            var result = await _positionRepositry.CreatePositionAsync(position);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie stanowiska", $"Występuje błąd w metodzie: {nameof(CreatePositionAsync)}", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false))
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie stanowiska", $"Występuje błąd w metodzie: {nameof(CreatePositionAsync)}", "Nie udało się utworzyć nowego stanowiska"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }
            return result.Result;
        }

        public async Task<bool> ChangeStatusEmployeePositionAsync(int? areaId, int? employeeId, ActivationStatus? positionStatus, int? userId)
        {
            if (areaId == null || employeeId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Edycja stanowiska", $"Parametr przekazany do metody ({nameof(ChangeStatusEmployeePositionAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(areaId) + nameof(employeeId));
            }

            var result = await _positionRepositry.ChangeStatusEmployeePositionAsync(areaId, employeeId, positionStatus);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Edycja stanowiska", $"Występuje błąd w metodzie: {nameof(ChangeStatusEmployeePositionAsync)}", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false))
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Edycja stanowiska", $"Występuje błąd w metodzie: {nameof(ChangeStatusEmployeePositionAsync)}", "Nie udało się edytować stanowisko"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }
            return result.Result;
        }

        public async Task<IEnumerable<Position>> GetPositionsAsync(int? userId)
        {
            var positions = await _positionRepositry.GetPositionsAsync();

            if (positions.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskanie danych o stanowiskach", $"Występuje błąd w metodzie: {nameof(GetPositionsAsync)}", positions.ErrorMassage));
                 throw new Exception(positions.ErrorMassage);
            }

            return positions.Result ?? throw new Exception("Error while getting data from table (Positions)");
        }

        public async Task<IEnumerable<Position>> GetPositionsByAreaAsync(int? areaId, int? userId)
        {
            if (areaId == null)
            {
                throw new ArgumentNullException(nameof(areaId));
            }

            var positions = await GetPositionsAsync(userId);

            return positions.Where(p => p.AreaId == areaId) ?? throw new Exception("Error while getting data from DB!");
        }

        public async Task<bool> DeactivateAllEmployeePositions(int? employeeId, int? userId)
        {
            if (employeeId == null || employeeId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Dezaktywacja stanowisk pracownika", $"Parametr przekazany do metody ({nameof(DeactivateAllEmployeePositions)}) ma wartość null"));
                throw new ArgumentNullException(nameof(employeeId));
            }

            var result = await _positionRepositry.DeactivateAllEmployeePositions(employeeId);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Dezaktywacja stanowisk pracownika", $"Występuje błąd w metodzie: {nameof(DeactivateAllEmployeePositions)}", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false))
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, $"Dezaktywacja stanowisk pracownika id:{employeeId}", $"Występuje błąd w metodzie: {nameof(DeactivateAllEmployeePositions)}", "Nie udalo się zrobić update w bazie"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }

            return result.Result;
        }

        public async Task<bool> SetAnEmployeeNewPosition(int? positionId, int? employeeId, int? userId)
        {
            if (positionId == null || employeeId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Aktualizacja stanowisk pracownika", $"Parametr przekazany do metody ({nameof(SetAnEmployeeNewPosition)}) ma wartość null"));
                throw new ArgumentNullException(nameof(positionId) + nameof(employeeId));
            }

            var result = await _positionRepositry.SetAnEmployeeNewPosition(positionId, employeeId);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Aktualizacja stanowisk pracownika", $"Występuje błąd w metodzie: {nameof(SetAnEmployeeNewPosition)}", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false))
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, $"Aktualizacja stanowisk pracownika id:{employeeId}", $"Występuje błąd w metodzie: {nameof(SetAnEmployeeNewPosition)}", "Nie udalo się zrobić update w bazie"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }

            return result.Result;
        }
    }
}
