using Data.LoggerRepository.Interface;
using Data.Repository.Form.Interface;
using Models.CreateModels;
using Services.ServiceAsync.Form.Interface;

namespace Services.ServiceAsync.Form
{
    public class AuditDataAsyncService : IAuditDataAsyncService
    {
        private readonly IAuditDataRepository _auditDataRepository;
        private readonly ILogger _logger;

        public AuditDataAsyncService(IAuditDataRepository auditDataRepository, ILogger logger)
        {
            _auditDataRepository = auditDataRepository;
            _logger = logger;
        }

        public async Task<int?> SaveAuditDataAsync(CreateAuditDocumentModel? data, int? userId)
        {
            if (data == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Zapisanie dokumentu audytu", $"Parametr przekazany do metody ({nameof(SaveAuditDataAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(data));
            }

            var result = await _auditDataRepository.SaveAuditDataAsync(data);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Zapisanie dokumentu audytu", $"Występuje błąd w metodzie ({nameof(SaveAuditDataAsync)})", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            return result.Result;
        }

        public async Task<bool> SaveAuditResultsAsync(AuditDocumentValues[]? data, int? userId)
        {
            if (data.Length == 0)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Zapisanie zawartości dokumentu audytu", $"Parametr przekazany do metody ({nameof(SaveAuditResultsAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(data));
            }

            var result = await _auditDataRepository.SaveAuditResultsAsync(data);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Zapisanie zawartości dokumentu audytu", $"Id Dokumentu: {data?.FirstOrDefault().AuditDocumentId}" , result.ErrorMassage ));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false))
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Zapisanie zawartości dokumentu audytu", $"Występuje błąd w metodzie ({nameof(SaveAuditResultsAsync)})", "Zawartość dokumentu audytu nie została zapisana"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }

            return result.Result;
        }
    }
}
