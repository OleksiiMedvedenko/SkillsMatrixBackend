using Data.LoggerRepository.Interface;
using Data.Repository.Interface;
using Models.CreateModels;
using Models.Model;
using Services.ServiceAsync.Interface;

namespace Services.ServiceAsync
{
    public class LevelDescriptionAsyncService : ILevelDescriptionAsyncService
    {
        private readonly ILevelDescriptionRepository _levelDescriptionRepository;
        private readonly ILogger _logger;

        public LevelDescriptionAsyncService(ILevelDescriptionRepository levelDescriptionRepository, ILogger logger)
        {
            _levelDescriptionRepository = levelDescriptionRepository;
            _logger = logger;
        }

        public async Task<bool> UpdateDescriptionAsyncService(EditLevelDescription[] descriptions, int? userId)
        {
            if (descriptions.FirstOrDefault().LevelDescriptionId == 0) 
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Edycja opisu poziomów audytu", $"Parametr przekazany do metody ({nameof(UpdateDescriptionAsyncService)}) ma wartość null"));
                throw new ArgumentNullException(nameof(descriptions));
            }

            var result = await _levelDescriptionRepository.UpdateLevelsDescriptionAsync(descriptions);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, $"Edycja opisu poziomów dla audytu pod identyfikatorem: {descriptions?.FirstOrDefault().AuditId}", $"Występuje błąd w metodzie ({nameof(UpdateDescriptionAsyncService)})", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false))
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, $"Edycja opisu poziomów dla audytu pod identyfikatorem: {descriptions?.FirstOrDefault().AuditId}", $"Występuje błąd w metodzie ({nameof(UpdateDescriptionAsyncService)})", "Opis nie był edytowany"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }

            return result.Result;
            
        }

        public async Task<IEnumerable<LevelDescription>> GetAuditLevelsDescriptionAsync(int? auditId, int? userId)
        {
            var descriptions = await _levelDescriptionRepository.GetAuditLevelsDescriptionAsync(auditId);

            if (descriptions.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskiwanie opisu poziomów audytu", $"Występuje błąd w metodzie: {nameof(GetAuditLevelsDescriptionAsync)}", descriptions.ErrorMassage));
                throw new Exception(descriptions.ErrorMassage);
            }

            if (descriptions?.Result?.Count() == 0)
            {
                //await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskiwanie opisu poziomów audytu", $"Występuje błąd w metodzie: {nameof(GetAuditLevelsDescriptionAsync)}", "brak danych"));
                throw new Exception("Error while getting data from DB");
            }

            return descriptions.Result;
        }

        public async Task<bool> CreateDescriptionAsyncService(int? auditId, int? userId)
        {
            var result = await _levelDescriptionRepository.CreateLevelsDescriptionAsync(auditId);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, $"Tworzenie opisu poziomu dla audytu: {auditId} id", $"Występuje błąd w metodzie: {nameof(CreateDescriptionAsyncService)}", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false)) 
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, $"Tworzenie opisu poziomu dla audytu: {auditId} id", $"Występuje błąd w metodzie: {nameof(CreateDescriptionAsyncService)}", "Nie udało się utworzyć opisu"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }

            return result.Result;
        }

        public async Task<IEnumerable<LevelDescription>> GetDepartmentAuditLevelDescriptionAsync(int? department, int? userId)
        {
            var descriptions = await _levelDescriptionRepository.GetDepartmentAuditLevelDescriptionAsync(department);

            if (descriptions.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskiwanie opisu poziomów dla audytów działu", $"Występuje błąd w metodzie: {nameof(GetDepartmentAuditLevelDescriptionAsync)}", descriptions.ErrorMassage));
                throw new Exception(descriptions.ErrorMassage);
            }

            if (descriptions?.Result?.Count() == 0)
            {
                //await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskiwanie opisu poziomów dla audytów działu", $"Występuje błąd w metodzie: {nameof(GetDepartmentAuditLevelDescriptionAsync)}", "brak danych"));
                throw new Exception("Error while getting data from DB");
            }

            return descriptions.Result;
        }
    }
}
