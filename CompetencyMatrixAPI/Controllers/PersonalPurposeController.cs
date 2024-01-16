using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CreateModels;
using Services.ServiceAsync.Interface;
using Tools.Serialization;

namespace CompetencyMatrixAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalPurposeController : ControllerBase
    {
        private readonly IPersonalPurposeAsyncService _personalPurposeAsyncService;

        public PersonalPurposeController(IPersonalPurposeAsyncService personalPurposeAsyncService)
        {
            _personalPurposeAsyncService = personalPurposeAsyncService;
        }

        [AllowAnonymous]
        [HttpGet("getDepartmentPersonalPurpose/{departmentId}")]
        public async Task<IActionResult> GetDepartmentPersonalPurpose(int? departmentId)
        {
            var data = await _personalPurposeAsyncService.GetDepartmentPersonalPurposeAsync(departmentId, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpGet("getDepartmentAuditsWithPurpose/{departmentId}")]
        public async Task<IActionResult> GetDepartmentAuditsWithPurpose(int? departmentId)
        {
            var data = await _personalPurposeAsyncService.GetDepartmentAuditsWithPurposeAsync(departmentId, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpPost("UpdateCreatePersonalPurpose")]
        public async Task<IActionResult> UpdateCreatePersonalPurpose(CreatePersonalPurpose? createPersonalPurpose)
        {
            if (createPersonalPurpose == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            var result = await _personalPurposeAsyncService.CreateOrUpdatePersonalPurposeAsync(createPersonalPurpose, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status201Created, JSONSerialization.ConvertToJSON(result));
        }
    }
}
