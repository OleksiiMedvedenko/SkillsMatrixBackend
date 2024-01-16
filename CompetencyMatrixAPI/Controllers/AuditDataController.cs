using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CreateModels;
using Services.ServiceAsync.Form.Interface;
using Tools.Serialization;

namespace CompetencyMatrixAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditDataController : ControllerBase
    {
        private readonly IAuditDataAsyncService _auditDataAsyncService;

        public AuditDataController(IAuditDataAsyncService auditDataAsyncService)
        {
            _auditDataAsyncService = auditDataAsyncService;
        }

        [AllowAnonymous]
        [HttpPost("createDocument")]
        public async Task<IActionResult> CreateDocument(CreateAuditDocumentModel? data)
        {
            if (data == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            var userId = HttpContext.Items["userId"] as int?;


            var result = await _auditDataAsyncService.SaveAuditDataAsync(data, userId);
            return StatusCode(StatusCodes.Status201Created, JSONSerialization.ConvertToJSON(result));
        }

        [AllowAnonymous]
        [HttpPost("saveAuditResult")]
        public async Task<IActionResult> SaveAuditResult(AuditDocumentValues[]? data)
        {
            if (data.Length == 0)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            var userId = HttpContext.Items["userId"] as int?;
            var result = await _auditDataAsyncService.SaveAuditResultsAsync(data, userId);
            return StatusCode(StatusCodes.Status201Created, JSONSerialization.ConvertToJSON(result));
        }
    }
}
