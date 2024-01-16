using Models.CreateModels;
using Models.Model;
using Models.ViewModels;
using Tools.DataService;

namespace Data.Repository.Form.Interface
{
    public interface IFormRepository
    {
        Task<ExternalDataResultManager<bool>> CreateTemplateAsync(CreateTemplateModel[]? questionsData);
        Task<ExternalDataResultManager<bool>> CreateTemplateHeaderAsync(HeaderTemplate? headerForm);
        Task<ExternalDataResultManager<HeaderTemplate>> GetTemplateHeaderAsync(int? templateId);
        Task<ExternalDataResultManager<IEnumerable<TemplateFormViewModel>>> GetCurrentTemplateFormAsync(int? auditId);
        Task<ExternalDataResultManager<IEnumerable<TemplateFormViewModel>>> GetCompletedTemplateFormAsync(int? auditHistoryId);
    }
}