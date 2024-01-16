using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.ServiceAsync.Authorization.Interface;
using Tools.Serialization;

namespace CompetencyMatrixAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationAsyncService _authorizationAsyncService;

        public AuthorizationController(IAuthorizationAsyncService authorizationAsyncService)
        {
            _authorizationAsyncService = authorizationAsyncService;
        }


        [AllowAnonymous]
        [HttpGet("Login/{login}/{password}")]
        public async Task<IActionResult> Login(string? login, string? password)
        {
            var data = await _authorizationAsyncService.Login(login, password);

            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }
    }
}
