using Models.CreateModels;
using Models.Model;
using Models.ViewModels;
using Tools.DataService;

namespace Services.ServiceAsync.Interface
{
    public interface IAuditAsyncService
    {
        Task<int> CreateAuditTypeAsync(ManagerAuditTypeModel? audit, int? userId);
        Task<bool> UpdateAuditTypeAsync(ManagerAuditTypeModel? audit, int? userId);


        Task<int?> UpdateAuditInfoAsync(UpdateAuditModel? newCompetency, int? userId);
        Task<bool> ReadAuditNotificationAsync(int? auditId, int? userId);

        Task<Audit> GetAuditAsync(int? auditId, int? userId);
        Task<PersonalCompetencyViewModel> GetPersonalCompetenceAsync(int? employeeId, int? departmentId, int? userId);
        Task<Audit> GetAuditCompetenceForEmployeeAsync(int? employeeId, int? auditId, int? userId);

        Task<IEnumerable<CompetencyViewModel>> GetDepartmentSoonAuditsAsync(int? departmentId, int? userId);
        Task<IEnumerable<CompetencyViewModel>> GetDepartmentFutureAuditsAsync(int? departmentId, int? userId);

        Task<IEnumerable<Audit>> GetAuditsAsync(int? userId);
        // reports 
        Task<IEnumerable<PersonalCompetencyViewModel>> GetFutureAreaAuditsDatesAsync(int? areaId, int? userId);
        Task<IEnumerable<PersonalCompetencyViewModel>> GetActualAreaAuditsDatesAsync(int? areaId, int? userId);
        Task<IEnumerable<PersonalCompetencyViewModel>> GetActualAreaAuditsLevelAsync(int? areaId, int? userId);
        Task<IEnumerable<PersonalCompetencyViewModel>> GetValuationAreaReportAsync(int? areaId, int? userId);

    }
}
