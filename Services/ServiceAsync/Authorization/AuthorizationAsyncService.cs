using Data.Repository.Authorization.Interface;
using Microsoft.AspNetCore.Http;
using Models.Model;
using Models.ViewModels;
using Services.ServiceAsync.Authorization.Interface;
using System.Net.Http.Headers;

namespace Services.ServiceAsync.Authorization
{
    public class AuthorizationAsyncService : IAuthorizationAsyncService
    {
        private readonly IAutorizationRepository _autorizationRepository;
        private readonly ILoginRepository _loginRepository;

        public AuthorizationAsyncService(IAutorizationRepository autorizationRepository, ILoginRepository loginRepository)
        {
            _autorizationRepository = autorizationRepository;
            _loginRepository = loginRepository;
        }

        public async Task<EmployeeViewModel> Authentication(string? login)
        {
            if (login == null )
            {
                throw new ArgumentNullException($"{nameof(login)}");
            }

            var data = await _autorizationRepository.Authentication(login);

            if (data.ErrorExist)
            {
                throw new Exception(data.ErrorMassage);
            }

            if (data.Result == null)
            {
                throw new Exception("Error getting data from Database!");
            }

            return data.Result;
        }

        public async Task<EmployeeViewModel?> Login(string? login, string? password)
        {
            if (login == null && password == null) 
            {
                throw new ArgumentNullException(nameof(login) + " and " + nameof(password));
            }

            var result = await _loginRepository.Login(login, password);

            if (result.ErrorExist)
            {
                throw new Exception(result.ErrorMassage);
            }

            if (result?.Result == -1)
            {
                throw new Exception("Not Found!");
            }

            return await Authentication(login);
        }
    }
}
