using Models.CreateModels;
using Models.Model;
using Tools.DataService;

namespace Services.ServiceAsync.Interface
{
    public interface ILevelDescriptionAsyncService
    {
        Task<bool> UpdateDescriptionAsyncService(EditLevelDescription[] descriptions, int? userId);
        Task<bool> CreateDescriptionAsyncService(int? auditId, int? userId);

        Task<IEnumerable<LevelDescription>> GetAuditLevelsDescriptionAsync(int? auditId, int? userId);
        Task<IEnumerable<LevelDescription>> GetDepartmentAuditLevelDescriptionAsync(int? department, int? userId);

    }
}
