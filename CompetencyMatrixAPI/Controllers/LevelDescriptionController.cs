using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CreateModels;
using Models.Model;
using Services.ServiceAsync.Interface;
using Tools.Serialization;

namespace CompetencyMatrixAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelDescriptionController : ControllerBase
    {
        private readonly ILevelDescriptionAsyncService _levelDescriptionAsyncService;

        public LevelDescriptionController(ILevelDescriptionAsyncService levelDescriptionAsyncService)
        {
            _levelDescriptionAsyncService = levelDescriptionAsyncService;
        }


        [AllowAnonymous]
        [HttpGet("getLevelDescription/{auditId}")]
        public async Task<IActionResult> GetLevelDescription(int? auditId)
        {
            var data = await _levelDescriptionAsyncService.GetAuditLevelsDescriptionAsync(auditId, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpGet("GetDepartmentAuditLevelDescription/{departmentId}")]
        public async Task<IActionResult> GetDepartmentAuditLevelDescription(int? departmentId)
        {
            var data = await _levelDescriptionAsyncService.GetDepartmentAuditLevelDescriptionAsync(departmentId, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpPost("updateLevelDescriptionInfo")]
        public async Task<IActionResult> UpdateLevelDescriptionInfo(EditLevelDescription[]? data)
        {
            if (data == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            var result = await _levelDescriptionAsyncService.UpdateDescriptionAsyncService(data, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status201Created, JSONSerialization.ConvertToJSON(result));
        }
    }
}

