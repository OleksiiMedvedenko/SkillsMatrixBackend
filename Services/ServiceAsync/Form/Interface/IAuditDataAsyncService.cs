using Models.CreateModels;

namespace Services.ServiceAsync.Form.Interface
{
    public interface IAuditDataAsyncService
    {
        Task<int?> SaveAuditDataAsync(CreateAuditDocumentModel? data, int? userId);
        Task<bool> SaveAuditResultsAsync(AuditDocumentValues[]? data, int? userId);
    }
}
