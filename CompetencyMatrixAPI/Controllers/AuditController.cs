using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.CreateModels;
using Models.ViewModels;
using Newtonsoft.Json;
using Services.ServiceAsync.Interface;
using Tools.Serialization;

namespace CompetencyMatrixAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IAuditAsyncService _auditService;

        public AuditController(IAuditAsyncService auditAsyncService)
        {
            _auditService = auditAsyncService;
        }

        [AllowAnonymous]
        [HttpGet("getAreaCompetencies/{areaId}")]
        public async Task<IActionResult> GetAreaCompetencies(int? areaId)
        {
            var data = await _auditService.GetActualAreaAuditsDatesAsync(areaId, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpGet("getActualAreaAuditsLevel/{areaId}")]
        public async Task<IActionResult> GetActualAreaAuditsLevel(int? areaId)
        {
            var data = await _auditService.GetActualAreaAuditsLevelAsync(areaId, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpGet("getFutureAreaAuditsDates/{areaId}")]
        public async Task<IActionResult> GetFutureAreaAuditsDates(int? areaId)
        {
            var data = await _auditService.GetFutureAreaAuditsDatesAsync(areaId, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpGet("getAudits")]
        public async Task<IActionResult> GetAudits()
        {
            var data = await _auditService.GetAuditsAsync(HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpGet("getPersonalCompetence/{employeeId}/{departmentId}")]
        public async Task<IActionResult> GetPersonalCompetence(int? employeeId, int? departmentId)
        {
            var data = await _auditService.GetPersonalCompetenceAsync(employeeId, departmentId, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data.AuditsInfo));
        }

        [AllowAnonymous]
        [HttpPut("readAuditNotification/{auditId}")]
        public async Task<IActionResult> ReadAuditNotificationPut(int? auditId)
        {
            var data = await _auditService.ReadAuditNotificationAsync(auditId, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status202Accepted, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpPost("updateAuditInfo")]
        public async Task<IActionResult> UpdateAuditInfoPost(UpdateAuditModel? newCompetency)
        {
            if (newCompetency == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
            var result = await _auditService.UpdateAuditInfoAsync(newCompetency, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status202Accepted, JSONSerialization.ConvertToJSON(result));
        }

        [AllowAnonymous]
        [HttpGet("getSoonAudits/{departmentId}")]
        public async Task<IActionResult> GetSoonAudits(int? departmentId)
        {
            var data = await _auditService.GetDepartmentSoonAuditsAsync(departmentId, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpGet("getFutureAudits/{departmentId}")]
        public async Task<IActionResult> GetFutureAudits(int? departmentId)
        {
            var data = await _auditService.GetDepartmentFutureAuditsAsync(departmentId, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpGet("getValuationArea/{areaId}")]
        public async Task<IActionResult> GetValuationArea(int? areaId)
        {
            var data = await _auditService.GetValuationAreaReportAsync(areaId, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpGet("getAuditCompetenceForEmployee/{employeeId}/{areaId}")]
        public async Task<IActionResult> GetAuditCompetenceForEmployee(int? employeeId, int? areaId)
        {
            var data = await _auditService.GetAuditCompetenceForEmployeeAsync(employeeId, areaId, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpPost("createAuditType")]
        public async Task<IActionResult> CreateAuditTypePost(ManagerAuditTypeModel? audit)
        {
            if (audit == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
            var result = await _auditService.CreateAuditTypeAsync(audit, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status202Accepted, JSONSerialization.ConvertToJSON(result));
        }

        [AllowAnonymous]
        [HttpPost("updateAuditType")]
        public async Task<IActionResult> UpdateAuditType(ManagerAuditTypeModel? audit)
        {
            if (audit == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
            var result = await _auditService.UpdateAuditTypeAsync(audit, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status202Accepted, JSONSerialization.ConvertToJSON(result));
        }
    }
}
