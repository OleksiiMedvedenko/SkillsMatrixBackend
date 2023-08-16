using Data.Repository;
using Data.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Models.CreateModels;
using Models.Model;
using Models.ViewModels;
using Services.ServiceAsync.Interface;

namespace Services.ServiceAsync
{
    public class AuditAsyncService : IAuditAsyncService
    {
        private readonly IAuditRepository _auditRepository;
        private readonly IEmployeeAsyncService _employeeService;

        public AuditAsyncService(IAuditRepository audiRepo, IEmployeeAsyncService employeeService)
        {
            _auditRepository = audiRepo;
            _employeeService = employeeService;
        }

        public async Task<IEnumerable<Audit>> GetAuditsAsync()
        {
            var audits = await _auditRepository.GetAuditsAsync();

            if (audits.ErrorExist)
            {
                throw new Exception(audits.ErrorMassage);
            }

            return audits.Result ?? throw new Exception("Error while getting data from table (Audit)");
        }

        public async Task<Audit> GetAuditAsync(int? auditId)
        {
            if (auditId == null)
            {
                { throw new ArgumentNullException(nameof(auditId)); }
            }

            var audits = await GetAuditsAsync();

            var audit = audits.SingleOrDefault(a => a.AuditId == auditId);

            return audit ?? throw new Exception("Error getting audit from database or audit does not exist!");
        }

        public async Task<IEnumerable<PersonalCompetencyViewModel>> GetActualAreaAuditsDatesAsync(int? areaId)
        {
            if (areaId == null)
            {
                { throw new ArgumentNullException(nameof(areaId)); }
            }

            var audits = await _auditRepository.GetActualAreaAuditsDatesAsync(areaId);

            if (audits.ErrorExist)
            {
                throw new Exception(audits.ErrorMassage);
            }

            return audits.Result ?? throw new Exception("Error getting audits from procedure(datesRaportByArea)!");
        }

        public async Task<PersonalCompetencyViewModel> GetPersonalCompetenceAsync(int? employeeId, int? departmentId)
        {
            if (employeeId == null || departmentId == null)
            {
                throw new ArgumentNullException($"{nameof(employeeId)} {Environment.NewLine} {nameof(departmentId)}"); 
            }

            var competence = await _auditRepository.GetPersonalCompetenceAsync(employeeId, departmentId);

            if (competence.ErrorExist)
            {
                throw new Exception(competence.ErrorMassage);
            }

            if (competence.Result == null)
            {
                throw new Exception("Error getting audits from Database!");
            }

            #region GeT employee data
            var emplo = await _employeeService.GetEmployeeAsync(employeeId);
            var personalCompetence = new PersonalCompetencyViewModel(emplo.Employee, emplo.Position, emplo.Supervisor);
            personalCompetence.AuditsInfo = competence?.Result?.AuditsInfo;
            #endregion

            return personalCompetence;
        }

        public async Task<bool> ReadAuditNotificationAsync(int? auditId)
        {
            if (auditId == null)
            {
                throw new ArgumentNullException(nameof(auditId));
            }

            var result = await _auditRepository.ReadAuditNotificationAsync(auditId);

            if (result.ErrorExist)
            {
                throw new Exception(result.ErrorMassage);
            }

            return result.Result.Equals(false)
                ? throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!")
                : result.Result;
        }

        public async Task<bool> UpdateAuditInfoAsync(UpdateAuditModel? newCompetency)
        {
            if (newCompetency == null)
            {
                throw new ArgumentNullException(nameof(newCompetency));
            }

            var result = await _auditRepository.UpdateAuditInfoAsync(newCompetency);

            if (result.ErrorExist)
            {
                throw new Exception(result.ErrorMassage);
            }

            return result.Result.Equals(false)
                ? throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!")
                : result.Result;
        }

        public async Task<IEnumerable<CompetencyViewModel>> GetDepartmentSoonAuditsAsync(int? departmentId)
        {
            if (departmentId == null)
            {
                { throw new ArgumentNullException(nameof(departmentId)); }
            }

            var audits = await _auditRepository.GetDepartmentSoonAuditsAsync(departmentId);

            if (audits.ErrorExist)
            {
                throw new Exception(audits.ErrorMassage);
            }

            return audits.Result ?? throw new Exception("Error getting audits from view(vLastView)!");
        }

        public async Task<Audit> GetAuditCompetenceForEmployeeAsync(int? employeeId, int? auditId)
        {
            if (employeeId == null || auditId == null)
            {
                throw new ArgumentNullException($"{nameof(employeeId)} {Environment.NewLine} {nameof(auditId)}");
            }

            var audit = await _auditRepository.GetAuditCompetenceForEmployeeAsync(employeeId, auditId);

            if (audit.ErrorExist)
            {
                throw new Exception(audit.ErrorMassage);
            }

            return audit.Result ?? throw new Exception("Error getting audits from view(vLastView)!");
        }

        public async Task<bool> CreateAuditTypeAsync(ManagerAuditTypeModel? audit)
        {
            if (audit == null)
            {
                throw new ArgumentNullException(nameof(audit));
            }

            var result = await _auditRepository.CreateAuditTypeAsync(audit);

            if (result.ErrorExist)
            {
                throw new Exception(result.ErrorMassage);
            }

            return result.Result.Equals(false)
                ? throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!")
                : result.Result;
        }

        public async Task<IEnumerable<PersonalCompetencyViewModel>> GetActualAreaAuditsLevelAsync(int? areaId)
        {
            if (areaId == null)
            {
                { throw new ArgumentNullException(nameof(areaId)); }
            }

            var audits = await _auditRepository.GetActualAreaAuditsLevelAsync(areaId);

            if (audits.ErrorExist)
            {
                throw new Exception(audits.ErrorMassage);
            }

            return audits.Result ?? throw new Exception("Error getting audits from procedure(levelRaportByArea)!");
        }

        public async Task<IEnumerable<CompetencyViewModel>> GetDepartmentFutureAuditsAsync (int? departmentId)
        {
            if (departmentId == null)
            {
                { throw new ArgumentNullException(nameof(departmentId)); }
            }

            var audits = await _auditRepository.GetDepartmentFutureAuditsAsync(departmentId);

            if (audits.ErrorExist)
            {
                throw new Exception(audits.ErrorMassage);
            }

            return audits.Result ?? throw new Exception("Error getting audits from view(vLastView)!");
        }

        public async Task<IEnumerable<PersonalCompetencyViewModel>> GetFutureAreaAuditsDatesAsync(int? areaId)
        {
            if (areaId == null)
            {
                { throw new ArgumentNullException(nameof(areaId)); }
            }

            var audits = await _auditRepository.GetFutureAreaAuditsDatesAsync(areaId);

            if (audits.ErrorExist)
            {
                throw new Exception(audits.ErrorMassage);
            }

            return audits.Result ?? throw new Exception("Error getting audits from procedure(datesRaportByArea)!");
        }

        public async Task<bool> UpdateAuditTypeAsync(ManagerAuditTypeModel? audit)
        {
            if (audit == null)
            {
                throw new ArgumentNullException(nameof(audit));
            }

            var result = await _auditRepository.UpdateAuditTypeAsync(audit);

            if (result.ErrorExist)
            {
                throw new Exception(result.ErrorMassage);
            }

            return result.Result.Equals(false)
                ? throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!")
                : result.Result;
        }

        public async Task<IEnumerable<PersonalCompetencyViewModel>> GetValuationAreaReportAsync(int? areaId)
        {
            if (areaId == null)
            {
                { throw new ArgumentNullException(nameof(areaId)); }
            }

            var audits = await _auditRepository.GetValuationAreaReportAsync(areaId);

            if (audits.ErrorExist)
            {
                throw new Exception(audits.ErrorMassage);
            }

            return audits.Result ?? throw new Exception("Error getting audits from procedure([dbo].[valuationReportByArea])!");
        }
    }
}
