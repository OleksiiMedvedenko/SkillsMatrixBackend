using Models.CreateModels;
using Tools.DataService;

namespace Data.Repository.Form.Interface
{
    public interface IAuditDataRepository
    {
        Task<ExternalDataResultManager<int?>> SaveAuditDataAsync(CreateAuditDocumentModel? data);
        Task<ExternalDataResultManager<bool>> SaveAuditResultsAsync(AuditDocumentValues[]? data);
    }
}
