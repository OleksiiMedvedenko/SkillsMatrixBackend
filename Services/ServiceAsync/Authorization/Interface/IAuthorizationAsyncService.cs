using Models.ViewModels;
using Tools.DataService;

namespace Services.ServiceAsync.Authorization.Interface
{
    public interface IAuthorizationAsyncService
    {
        Task<EmployeeViewModel?> Login(string? login, string? password);
        Task<EmployeeViewModel> Authentication(string? login);
    }
}
