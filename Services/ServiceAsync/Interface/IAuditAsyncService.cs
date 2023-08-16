using Models.CreateModels;
using Models.Model;
using Models.ViewModels;
using Tools.DataService;

namespace Services.ServiceAsync.Interface
{
    public interface IAuditAsyncService
    {
        Task<bool> CreateAuditTypeAsync(ManagerAuditTypeModel? audit);
        Task<bool> UpdateAuditTypeAsync(ManagerAuditTypeModel? audit);


        Task<bool> UpdateAuditInfoAsync(UpdateAuditModel? newCompetency);
        Task<bool> ReadAuditNotificationAsync(int? auditId);

        Task<Audit> GetAuditAsync(int? auditId);
        Task<PersonalCompetencyViewModel> GetPersonalCompetenceAsync(int? employeeId, int? departmentId);
        Task<Audit> GetAuditCompetenceForEmployeeAsync(int? employeeId, int? auditId);

        Task<IEnumerable<CompetencyViewModel>> GetDepartmentSoonAuditsAsync(int? departmentId);
        Task<IEnumerable<CompetencyViewModel>> GetDepartmentFutureAuditsAsync(int? departmentId);

        Task<IEnumerable<Audit>> GetAuditsAsync();
        // reports 
        Task<IEnumerable<PersonalCompetencyViewModel>> GetFutureAreaAuditsDatesAsync(int? areaId);
        Task<IEnumerable<PersonalCompetencyViewModel>> GetActualAreaAuditsDatesAsync(int? areaId);
        Task<IEnumerable<PersonalCompetencyViewModel>> GetActualAreaAuditsLevelAsync(int? areaId);
        Task<IEnumerable<PersonalCompetencyViewModel>> GetValuationAreaReportAsync(int? areaId);

    }
}
