using Data.LoggerRepository.Interface;
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
        private readonly ILevelDescriptionAsyncService _levelDescriptionAsyncService;
        private readonly ILogger _logger;

        public AuditAsyncService(IAuditRepository audiRepo, IEmployeeAsyncService employeeService, ILevelDescriptionAsyncService levelDescriptionAsyncService, ILogger logger)
        {
            _auditRepository = audiRepo;
            _employeeService = employeeService;
            _levelDescriptionAsyncService = levelDescriptionAsyncService;
            _logger = logger;
        }

        public async Task<IEnumerable<Audit>> GetAuditsAsync(int? userId)
        {
            var audits = await _auditRepository.GetAuditsAsync();

            if (audits.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pozyskiwanie danych o audytach", $"Występuje błąd w metodzie: {nameof(GetAuditsAsync)}", audits.ErrorMassage));
                throw new Exception(audits.ErrorMassage);
            }

            if (audits?.Result?.Count() == 0)
            {
                //await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pozyskiwanie danych o audytach", $"Występuje błąd w metodzie: {nameof(GetAuditsAsync)}", "Nie udało się pobrać danych z bazy/brak danych"));
                throw new Exception("Error while getting data from table (Audit)");
            }

            return audits.Result;
        }

        public async Task<Audit> GetAuditAsync(int? auditId, int? userId)
        {
            if (auditId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskanie danych o audycie", $"Parametr przekazany do metody ({nameof(GetAuditAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(auditId)); 
            }

            var audits = await GetAuditsAsync(userId);

            var audit = audits.SingleOrDefault(a => a.AuditId == auditId);

            if (audit == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskanie danych o audycie", $"Występuje błąd w metodzie: {nameof(GetAuditAsync)}", "Nie udało się pobrać danych o audycie/brak danych"));
                throw new Exception("Error getting audit from database or audit does not exist!");
            }

            return audit;
        }

        public async Task<IEnumerable<PersonalCompetencyViewModel>> GetActualAreaAuditsDatesAsync(int? areaId, int? userId)
        {
            if (areaId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pozyskiwanie danych o audytach", $"Parametr przekazany do metody ({nameof(GetActualAreaAuditsDatesAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(areaId));
            }

            var audits = await _auditRepository.GetActualAreaAuditsDatesAsync(areaId);

            if (audits.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pozyskiwanie danych o audytach", $"Występuje błąd w metodzie: {nameof(GetActualAreaAuditsDatesAsync)}", audits.ErrorMassage));
                throw new Exception(audits.ErrorMassage);
            }

            if (audits?.Result?.Count() == 0)
            {
                //await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pozyskiwanie danych o audytach", $"Występuje błąd w metodzie: {nameof(GetActualAreaAuditsDatesAsync)}", "Nie udało się pobrać danych z bazy/brak danych"));
                throw new Exception("Error getting audits from procedure(datesRaportByArea)!");
            }

            return audits.Result;
        }

        public async Task<PersonalCompetencyViewModel> GetPersonalCompetenceAsync(int? employeeId, int? departmentId, int? userId)
        {
            if (employeeId == null || departmentId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pozyskiwanie danych o kompetencji pracownika", $"Parametr przekazany do metody ({nameof(GetPersonalCompetenceAsync)}) ma wartość null"));
                throw new ArgumentNullException($"{nameof(employeeId)} {Environment.NewLine} {nameof(departmentId)}"); 
            }

            var competence = await _auditRepository.GetPersonalCompetenceAsync(employeeId, departmentId);

            if (competence.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pozyskiwanie danych o kompetencji pracownika", $"Występuje błąd w metodzie: {nameof(GetPersonalCompetenceAsync)}", competence.ErrorMassage));
                throw new Exception(competence.ErrorMassage);
            }

            if (competence.Result == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pozyskiwanie danych o kompetencji pracownika", $"Występuje błąd w metodzie: {nameof(GetPersonalCompetenceAsync)}", "Nie udało się pobrać danych o audycie/brak danych"));
                throw new Exception("Error getting audits from Database!");
            }

            #region GeT employee data
            var emplo = await _employeeService.GetEmployeeAsync(employeeId, userId);
            var personalCompetence = new PersonalCompetencyViewModel(emplo.Employee, emplo.Position, emplo.Supervisor);
            personalCompetence.AuditsInfo = competence?.Result?.AuditsInfo;
            #endregion

            return personalCompetence;
        }

        public async Task<bool> ReadAuditNotificationAsync(int? auditId, int? userId)
        {
            if (auditId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Przeczytanie powiadomienia", $"Parametr przekazany do metody ({nameof(ReadAuditNotificationAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(auditId));
            }

            var result = await _auditRepository.ReadAuditNotificationAsync(auditId);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Przeczytanie powiadomienia", $"Występuje błąd w metodzie: {nameof(ReadAuditNotificationAsync)}", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false))
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Przeczytanie powiadomienia", $"Występuje błąd w metodzie: {nameof(ReadAuditNotificationAsync)}", "Nie udalo się zrobić update w bazie"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }

            return result.Result;
        }

        public async Task<int?> UpdateAuditInfoAsync(UpdateAuditModel? newCompetency, int? userId)
        {
            if (newCompetency == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Zaktualizuj informacje o audycie(kompetencji) pracownika", $"Parametr przekazany do metody ({nameof(UpdateAuditInfoAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(newCompetency));
            }

            var result = await _auditRepository.UpdateAuditInfoAsync(newCompetency);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Zaktualizuj informacje o audycie(kompetencji) pracownika", $"Występuje błąd w metodzie: {nameof(UpdateAuditInfoAsync)}", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            return result.Result;
        }

        public async Task<IEnumerable<CompetencyViewModel>> GetDepartmentSoonAuditsAsync(int? departmentId, int? userId)
        {
            if (departmentId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskanie informacji o zbliżających się audytach", $"Parametr przekazany do metody ({nameof(GetDepartmentSoonAuditsAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(departmentId));
            }

            var audits = await _auditRepository.GetDepartmentSoonAuditsAsync(departmentId);

            if (audits.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskanie informacji o zbliżających się audytach", $"Występuje błąd w metodzie: {nameof(GetDepartmentSoonAuditsAsync)}", audits.ErrorMassage));
                throw new Exception(audits.ErrorMassage);
            }

            if (audits?.Result?.Count() == 0)
            {
                //await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskanie informacji o zbliżających się audytach", $"Występuje błąd w metodzie: {nameof(GetDepartmentSoonAuditsAsync)}", "Nie udało się pobrać danych o audycie/brak danych"));
                throw new Exception("Error getting audits from view(vLastView)!");
            }

            return audits.Result;
        }

        public async Task<Audit> GetAuditCompetenceForEmployeeAsync(int? employeeId, int? auditId, int? userId)
        {
            if (employeeId == null || auditId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pobieranie kompetencji pracownika dla audytu", $"Parametr przekazany do metody ({nameof(GetAuditCompetenceForEmployeeAsync)}) ma wartość null"));
                throw new ArgumentNullException($"{nameof(employeeId)} {Environment.NewLine} {nameof(auditId)}");
            }

            var audit = await _auditRepository.GetAuditCompetenceForEmployeeAsync(employeeId, auditId);

            if (audit.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pobieranie kompetencji pracownika dla audytu", $"Występuje błąd w metodzie: {nameof(GetAuditCompetenceForEmployeeAsync)}", audit.ErrorMassage));
                throw new Exception(audit.ErrorMassage);
            }

            if (audit?.Result == null) 
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pobieranie kompetencji pracownika dla audytu", $"Występuje błąd w metodzie: {nameof(GetAuditCompetenceForEmployeeAsync)}", "Nie udało się pobrać danych o kompetencji/brak danych"));
                throw new Exception("Error getting audits from view(vLastView)!");
            }
            return audit.Result;
        }

        public async Task<int> CreateAuditTypeAsync(ManagerAuditTypeModel? audit, int? userId)
        {
            if (audit == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie nowego typu audytu", $"Parametr przekazany do metody ({nameof(CreateAuditTypeAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(audit));
            }

            var result = await _auditRepository.CreateAuditTypeAsync(audit);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie nowego typu audytu", $"Występuje błąd w metodzie: {nameof(CreateAuditTypeAsync)}", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false))
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Tworzenie nowego typu audytu", $"Występuje błąd w metodzie: {nameof(CreateAuditTypeAsync)}", "Nie udało się utworzyć nowy typ audytu"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }

            await _levelDescriptionAsyncService.CreateDescriptionAsyncService(result.Result, userId);

            return result.Result;
        }

        public async Task<IEnumerable<PersonalCompetencyViewModel>> GetActualAreaAuditsLevelAsync(int? areaId, int? userId)
        {
            if (areaId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pobieranie kompetencji pracownika", $"Parametr przekazany do metody ({nameof(GetActualAreaAuditsLevelAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(areaId));
            }

            var audits = await _auditRepository.GetActualAreaAuditsLevelAsync(areaId);

            if (audits.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pobieranie kompetencji pracownika", $"Występuje błąd w metodzie: {nameof(GetActualAreaAuditsLevelAsync)}", audits.ErrorMassage));
                throw new Exception(audits.ErrorMassage);
            }

            if (audits.Result == null)
            {
                //await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Pobieranie kompetencji pracownika", $"Występuje błąd w metodzie: {nameof(GetActualAreaAuditsLevelAsync)}", "Nie udało się pobrać danych o kompetencji/brak danych"));
                throw new Exception("Error getting audits from procedure(levelRaportByArea)!");
            }

            return audits.Result;
        }

        public async Task<IEnumerable<CompetencyViewModel>> GetDepartmentFutureAuditsAsync (int? departmentId, int? userId)
        {
            if (departmentId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskanie informacji o przyszłych audytach", $"Parametr przekazany do metody ({nameof(GetDepartmentFutureAuditsAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(departmentId));
            }

            var audits = await _auditRepository.GetDepartmentFutureAuditsAsync(departmentId);

            if (audits.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskanie informacji o przyszłych audytach", $"Występuje błąd w metodzie: {nameof(GetDepartmentFutureAuditsAsync)}", audits.ErrorMassage));
                throw new Exception(audits.ErrorMassage);
            }

            if (audits?.Result?.Count() == 0)
            {
                //await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskanie informacji o przyszłych audytach", $"Występuje błąd w metodzie: {nameof(GetDepartmentFutureAuditsAsync)}", "Nie udało się pobrać danych o audycie/brak danych"));
                throw new Exception("Error getting audits from view(vLastView)!");
            }

            return audits.Result;
        }

        public async Task<IEnumerable<PersonalCompetencyViewModel>> GetFutureAreaAuditsDatesAsync(int? areaId, int? userId)
        {
            if (areaId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskanie informacji o przyszłych audytach", $"Parametr przekazany do metody ({nameof(GetFutureAreaAuditsDatesAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(areaId));
            }

            var audits = await _auditRepository.GetFutureAreaAuditsDatesAsync(areaId);

            if (audits.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskanie informacji o przyszłych audytach", $"Występuje błąd w metodzie: {nameof(GetFutureAreaAuditsDatesAsync)}", audits.ErrorMassage));
                throw new Exception(audits.ErrorMassage);
            }

            if (audits?.Result?.Count() == 0)
            {
                //await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskanie informacji o przyszłych audytach", $"Występuje błąd w metodzie: {nameof(GetFutureAreaAuditsDatesAsync)}", "Nie udało się pobrać danych o audycie/brak danych"));
                throw new Exception("Error getting audits from procedure(datesRaportByArea)!");
            }

            return audits.Result;
        }

        public async Task<IEnumerable<PersonalCompetencyViewModel>> GetValuationAreaReportAsync(int? areaId, int? userId)
        {
            if (areaId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, $"Uzyskanie raportu dla obszaru: {areaId} id", $"Parametr przekazany do metody ({nameof(GetValuationAreaReportAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(areaId));
            }

            var audits = await _auditRepository.GetValuationAreaReportAsync(areaId);

            if (audits.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, $"Uzyskanie raportu dla obszaru: {areaId} id", $"Występuje błąd w metodzie: {nameof(GetValuationAreaReportAsync)}", audits.ErrorMassage));
                throw new Exception(audits.ErrorMassage);
            }


            if (audits?.Result?.Count() == 0)
            {
                //await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, $"Uzyskanie raportu dla obszaru: {areaId} id", $"Występuje błąd w metodzie: {nameof(GetValuationAreaReportAsync)}", "Nie udało się pobrać danych o audycie/brak danych"));
                throw new Exception("Error getting audits from procedure([dbo].[valuationReportByArea])!");
            }

            return audits.Result;
        }

        public async Task<bool> UpdateAuditTypeAsync(ManagerAuditTypeModel? audit, int? userId)
        {
            if (audit == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, $"Aktualizacja danych typu audytu: {audit?.AuditId} id", $"Parametr przekazany do metody ({nameof(UpdateAuditTypeAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(audit));
            }

            var result = await _auditRepository.UpdateAuditTypeAsync(audit);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, $"Aktualizacja danych typu audytu: {audit?.AuditId} id", $"Występuje błąd w metodzie: {nameof(UpdateAuditTypeAsync)}", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            if (result.Result.Equals(false))
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, $"Aktualizacja danych typu audytu: {audit?.AuditId} id", $"Występuje błąd w metodzie: {nameof(UpdateAuditTypeAsync)}", "Nie udalo się zrobić update w bazie"));
                throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
            }

            return result.Result;
        }
    }
}
