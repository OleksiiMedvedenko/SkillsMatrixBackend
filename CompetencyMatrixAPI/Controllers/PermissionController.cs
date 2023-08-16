using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.CreateModels;
using Models.Model;
using Services.ServiceAsync.Interface;
using Tools.Serialization;

namespace CompetencyMatrixAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionAsyncService _permissionsnAsyncService;

        public PermissionController(IPermissionAsyncService permissionsnAsyncService)
        {
            _permissionsnAsyncService = permissionsnAsyncService;
        }

        [AllowAnonymous]
        [HttpGet("getPermissions")]
        public async Task<IActionResult> GetPermissions()
        {
            var data = await _permissionsnAsyncService.GetPermissionsAsync();
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpPost("createPermissions")]
        public async Task<IActionResult> CreatePermissions(Permission? permission)
        {
            if (permission == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            var result = await _permissionsnAsyncService.CreatePermissionAsync(permission);
            return StatusCode(StatusCodes.Status201Created, JSONSerialization.ConvertToJSON(result));
        }
    }
}
