using Models.CreateModels;
using Models.ViewModels;

namespace Services.ServiceAsync.Interface
{
    public interface IPersonalPurposeAsyncService
    {
        Task<IEnumerable<PersonalPurposeViewModel>> GetDepartmentAuditsWithPurposeAsync(int? departmentId, int? userId);
        Task<IEnumerable<PersonalPurposeViewModel>> GetDepartmentPersonalPurposeAsync(int? departmentId, int? userId);
        Task<bool> CreateOrUpdatePersonalPurposeAsync(CreatePersonalPurpose? createPersonalPurposes, int? userId);
    }
}
