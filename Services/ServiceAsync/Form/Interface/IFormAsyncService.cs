using Models.CreateModels;
using Models.Model;
using Models.ViewModels;
using Tools.DataService;

namespace Services.ServiceAsync.Form.Interface
{
    public interface IFormAsyncService
    {
        Task<bool> CreateTemplateHeaderAsync(HeaderTemplate? headerForm, int? userId);
        Task<bool> CreateTemplateAsync(CreateTemplateModel[]? questionsData, int? userId);

        Task<HeaderTemplate> GetTemplateHeaderAsync(int? templateId, int? userId);

        Task<IEnumerable<TemplateFormViewModel>> GetCurrentTemplateFormAsync(int? auditId, int? userId);
        Task<IEnumerable<TemplateFormViewModel>> GetCompletedTemplateFormAsync(int? auditHistoryId, int? userId);

    }
}
