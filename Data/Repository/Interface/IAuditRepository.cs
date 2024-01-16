using Models.CreateModels;
using Models.Model;
using Models.ViewModels;
using Tools.DataService;

namespace Data.Repository.Interface
{
    public interface IAuditRepository
    {

        Task<ExternalDataResultManager<int>> CreateAuditTypeAsync(ManagerAuditTypeModel? audit);
        Task<ExternalDataResultManager<bool>> UpdateAuditTypeAsync(ManagerAuditTypeModel? audit);
        
        Task<ExternalDataResultManager<int?>> UpdateAuditInfoAsync(UpdateAuditModel? newCompetency);
        Task<ExternalDataResultManager<bool>> ReadAuditNotificationAsync(int? auditId);

        /// <summary>
        /// Get only audits with DATE AND LEVEL, append with employee data
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="department"></param>
        /// <returns></returns>
        Task<ExternalDataResultManager<PersonalCompetencyViewModel>> GetPersonalCompetenceAsync(int? employeeId, int? departmentId);

        Task<ExternalDataResultManager<Audit>> GetAuditCompetenceForEmployeeAsync(int? employeeId, int? areaId);

        //Get comming soon audits 
        Task<ExternalDataResultManager<IEnumerable<CompetencyViewModel>>> GetDepartmentSoonAuditsAsync(int? departmentId);
        Task<ExternalDataResultManager<IEnumerable<CompetencyViewModel>>> GetDepartmentFutureAuditsAsync(int? departmentId);

        Task<ExternalDataResultManager<IEnumerable<Audit>>> GetAuditsAsync();

        //procedures/ raports 
        Task<ExternalDataResultManager<IEnumerable<PersonalCompetencyViewModel>>> GetFutureAreaAuditsDatesAsync(int? areaId);
        Task<ExternalDataResultManager<IEnumerable<PersonalCompetencyViewModel>>> GetActualAreaAuditsDatesAsync(int? areaId);
        Task<ExternalDataResultManager<IEnumerable<PersonalCompetencyViewModel>>> GetActualAreaAuditsLevelAsync(int? areaId);
        Task<ExternalDataResultManager<IEnumerable<PersonalCompetencyViewModel>>> GetValuationAreaReportAsync(int? areaId);
     }
}
