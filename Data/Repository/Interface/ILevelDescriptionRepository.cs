using Models.CreateModels;
using Models.Model;
using Tools.DataService;

namespace Data.Repository.Interface
{
    public interface ILevelDescriptionRepository
    {
        Task<ExternalDataResultManager<bool>> CreateLevelsDescriptionAsync(int? auditId);
        Task<ExternalDataResultManager<bool>> UpdateLevelsDescriptionAsync(EditLevelDescription[] descriptions);

        Task<ExternalDataResultManager<IEnumerable<LevelDescription>>> GetAuditLevelsDescriptionAsync(int? auditId);
        Task<ExternalDataResultManager<IEnumerable<LevelDescription>>> GetDepartmentAuditLevelDescriptionAsync(int? department);

    }
}
