using Data.LoggerRepository.Interface;
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
        private readonly ILogger _logger;

        public AuthorizationAsyncService(IAutorizationRepository autorizationRepository, ILoginRepository loginRepository, ILogger logger)
        {
            _autorizationRepository = autorizationRepository;
            _loginRepository = loginRepository;
            _logger = logger;
        }

        public async Task<EmployeeViewModel> Authentication(string? login)
        {
            if (login == null )
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(null, "Logowanie" ,$"Parametr 'login' przekazany do metody ({nameof(Authentication)}) ma wartość null"));

                throw new ArgumentNullException($"{nameof(login)}");
            }

            var data = await _autorizationRepository.Authentication(login);

            if (data.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(null, "Logowanie", $"Metoda: ({nameof(Authentication)})| Login: {login}", data.ErrorMassage));

                throw new Exception(data.ErrorMassage);
            }

            if (data.Result == null)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(null, "Logowanie", $"Dane są pobierane po wywołaniu metody ({nameof(Authentication)}) o wartości null. Przekazany parametr 'login' = {login}"));

                throw new Exception("Error getting data from Database!");
            }

            return data.Result;
        }

        public async Task<EmployeeViewModel?> Login(string? login, string? password)
        {
            if (login == null && password == null) 
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(null, "Pobierania użytkownika według procedury [user.logIn]", $"Parametry przekazane do procedury mają wartość null (login: {login} ) w metodzie: ({nameof(Login)})"));

                throw new ArgumentNullException(nameof(login) + " and " + nameof(password));
            }

            var result = await _loginRepository.Login(login, password);

            if (result.ErrorExist)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(null, $"Pobierania użytkownika według procedury [user.logIn] w metodzie: ({nameof(Login)})", login, result.ErrorMassage));

                throw new Exception(result.ErrorMassage);
            }

            if (result?.Result == -1)
            {
                await _logger.DeclareErrorAsync(new Models.AppModel.LoggerModel(null, "Pobierania użytkownika według procedury [user.logIn]", $"Dane są pobierane po wywołaniu procedury [user.logIn] o wartości null. Przekazany parametr 'login' = {login} w metodzie: ({nameof(Login)})"));
                throw new Exception("Not Found!");
            }

            return await Authentication(login);
        }
    }
}
