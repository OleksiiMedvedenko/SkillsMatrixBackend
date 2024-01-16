using Data.LoggerRepository.Interface;
using Data.Repository;
using Data.Repository.Interface;
using Models.CreateModels;
using Models.Model;
using Models.ViewModels;
using Services.ServiceAsync.Interface;
using Tools.DataService;

namespace Services.ServiceAsync
{
    public class PersonalPurposeAsyncService : IPersonalPurposeAsyncService
    {
        private readonly IPersonalPurposeRepository _personalPurposeRepository;
        private readonly ILogger _logger;

        public PersonalPurposeAsyncService(IPersonalPurposeRepository personalPurposeRepository, ILogger logger)
        {
            _personalPurposeRepository = personalPurposeRepository;
            _logger = logger;
        }

        public async Task<bool> CreateOrUpdatePersonalPurposeAsync(CreatePersonalPurpose? createPersonalPurposes, int? userId)
        {
            if (createPersonalPurposes == null || createPersonalPurposes?.AuditId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Employee demand", $"Parametr przekazany do metody ({nameof(CreateOrUpdatePersonalPurposeAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(createPersonalPurposes));
            }

            var departmentAudits = await GetDepartmentAuditsWithPurposeAsync(createPersonalPurposes?.DepartmentId, userId);
            var checkOnExist = departmentAudits.FirstOrDefault(x => x.Audit?.AuditId == createPersonalPurposes.AuditId);

            if (checkOnExist.Purpose != null)
            {
                var result = await _personalPurposeRepository.UpdatePersonalPurposeAsync(createPersonalPurposes);

                if (result.ErrorExist)
                {
                    await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "(Update) Employee demand", $"Występuje błąd w metodzie: {nameof(CreateOrUpdatePersonalPurposeAsync)}", result.ErrorMassage));
                    throw new Exception(result.ErrorMassage);
                }

                if(result.Result.Equals(false))
                {
                    await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "(Update) Employee demand", $"Występuje błąd w metodzie: {nameof(CreateOrUpdatePersonalPurposeAsync)}", null));
                    throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
                }

                return result.Result;
            }
            else
            {
                var result = await _personalPurposeRepository.CreatePersonalPurposeAsync(createPersonalPurposes);

                if (result.ErrorExist)
                {
                    await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "(Create) Employee demand", $"Występuje błąd w metodzie: {nameof(CreateOrUpdatePersonalPurposeAsync)}", result.ErrorMassage));
                    throw new Exception(result.ErrorMassage);
                }

                if (result.Result.Equals(false))
                {
                    await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "(Create) Employee demand", $"Występuje błąd w metodzie: {nameof(CreateOrUpdatePersonalPurposeAsync)}", null));
                    throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
                }

                return result.Result;
            }
        }

        public async Task<IEnumerable<PersonalPurposeViewModel>> GetDepartmentAuditsWithPurposeAsync(int? departmentId, int? userId)
        {
            if (departmentId == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskiwanie audytów działów z zapotrzebowaniem na pracowników", $"Parametr przekazany do metody ({nameof(GetDepartmentAuditsWithPurposeAsync)}) ma wartość null"));
                throw new ArgumentNullException(nameof(departmentId));
            }
            var result = await _personalPurposeRepository.GetDepartmentAuditsWithPurposeAsync(departmentId);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, "Uzyskiwanie audytów działów z zapotrzebowaniem na pracowników", $"Występuje błąd w metodzie: {nameof(GetDepartmentAuditsWithPurposeAsync)}", result.ErrorMassage));
                throw new Exception(result.ErrorMassage);
            }

            return result.Result ?? throw new Exception("The operation was not completed, something went wrong, try again, if the operation fails again - write to the administrator!");
        }

        public async Task<IEnumerable<PersonalPurposeViewModel>> GetDepartmentPersonalPurposeAsync(int? departmentId, int? userId)
        {
            var descriptions = await _personalPurposeRepository.GetDepartmentPersonalPurposeAsync(departmentId);

            if (descriptions.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(userId, $"Uzyskiwanie zapotrzebowania na pracowników dla działu: {departmentId} id", $"Występuje błąd w metodzie: {nameof(GetDepartmentPersonalPurposeAsync)}", descriptions.ErrorMassage));
                throw new Exception(descriptions.ErrorMassage);
            }

            return descriptions.Result ?? throw new Exception("Error while getting data from DB");
        }
    }
}
