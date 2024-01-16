using Models.CreateModels;
using Models.ViewModels;
using Tools.DataService;

namespace Data.Repository.Interface
{
    public interface IPersonalPurposeRepository
    {
        Task<ExternalDataResultManager<IEnumerable<PersonalPurposeViewModel>>> GetDepartmentAuditsWithPurposeAsync(int? departmentId);
        Task<ExternalDataResultManager<IEnumerable<PersonalPurposeViewModel>>> GetDepartmentPersonalPurposeAsync(int? departmentId);
        Task<ExternalDataResultManager<bool>> UpdatePersonalPurposeAsync(CreatePersonalPurpose? createPersonalPurposes);
        Task<ExternalDataResultManager<bool>> CreatePersonalPurposeAsync(CreatePersonalPurpose? createPersonalPurposes);
    }
}
