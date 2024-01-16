using Data.LoggerRepository.Interface;
using Data.Repository.Form.Interface;
using Models.CreateModels;
using Models.Model;
using Models.ViewModels;
using Services.ServiceAsync.Form.Interface;

namespace Services.ServiceAsync.Form
{
    public class FormAsyncService : IFormAsyncService
    {
        private readonly IFormRepository _formRepository;
        private readonly ILogger _logger;

        public FormAsyncService(IFormRepository formRepository, ILogger logger)
        {
            _formRepository = formRepository;
            _logger = logger;
        }

        public async Task<bool> CreateTemplateHeaderAsync(HeaderTemplate? headerForm, int? userId)
        {
            if (headerForm == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie nagłówka formularza audytu", $"Parametr przekazany do metody ({nameof(CreateTemplateHeaderAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(headerForm));
            }

            var result = await _formRepository.CreateTemplateHeaderAsync(headerForm);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie nagłówka formularza audytu", $"Id formularza: {headerForm?.TemplateId}", result?.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false))
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie nagłówka formularza audytu", $"Występuje błąd w metodzie ({nameof(CreateTemplateHeaderAsync)})", "Naglówek formularza audytu nie został stworzony"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }
            return result.Result;
        }

        public async Task<bool> CreateTemplateAsync(CreateTemplateModel[]? questionsData, int? userId)
        {
            if (questionsData.Length == 0)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie formularza audytu", $"Parametr przekazany do metody ({nameof(CreateTemplateAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(questionsData));
            }

            var result = await _formRepository.CreateTemplateAsync(questionsData);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie formularza audytu", $"Id audytu, dla którego ten formularz: {questionsData?.FirstOrDefault()?.AuditId}", result?.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false))
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie formularza audytu", $"Występuje błąd w metodzie ({nameof(CreateTemplateAsync)})", "Formularz audytu nie został stworzony"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }

            return result.Result;
        }

        public async Task<IEnumerable<TemplateFormViewModel>> GetCurrentTemplateFormAsync(int? auditId, int? userId)
        {
            var template = await _formRepository.GetCurrentTemplateFormAsync(auditId);

            if (template.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pobieranie formularza z bazy", $"Id audytu, dla którego pobieramy formularz: {auditId}", template?.ErrorMassage));
                throw new Exception(template.ErrorMassage);
            }

            return template.Result;
        }

        public async Task<HeaderTemplate> GetTemplateHeaderAsync(int? templateId, int? userId)
        {
            var templateHeader = await _formRepository.GetTemplateHeaderAsync(templateId);

            if (templateHeader.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pobieranie danych naglówka formularza z bazy", $"Id formularza, dla którego pobieramy naglówek: {templateId}", templateHeader?.ErrorMassage));
                throw new Exception(templateHeader.ErrorMassage);
            }

            return templateHeader.Result;
        }

        public async Task<IEnumerable<TemplateFormViewModel>> GetCompletedTemplateFormAsync(int? auditHistoryId, int? userId)
        {
            var template = await _formRepository.GetCompletedTemplateFormAsync(auditHistoryId);

            if (template.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pobieranie danych formularza z bazy", $"auditHistoryId: {auditHistoryId}", template?.ErrorMassage));
                throw new Exception(template.ErrorMassage);
            }

            return template.Result;
        }
    }
}
