using Models.ViewModels;
using Tools.DataService;

namespace Data.Repository.Authorization.Interface
{
    public interface IAutorizationRepository
    {
        Task<ExternalDataResultManager<EmployeeViewModel>> Authentication(string? login);
    }
}
