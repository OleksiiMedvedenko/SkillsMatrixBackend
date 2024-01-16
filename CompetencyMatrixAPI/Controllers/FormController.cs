using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CreateModels;
using Models.Model;
using Services.ServiceAsync.Form.Interface;
using Tools.Serialization;

namespace CompetencyMatrixAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormController : ControllerBase
    {
        private readonly IFormAsyncService _formAsyncService;

        public FormController(IFormAsyncService formAsyncService)
        {
            _formAsyncService = formAsyncService;
        }

        [AllowAnonymous]
        [HttpGet("getTemplate/{auditId}")]
        public async Task<IActionResult> GetTemplate(int? auditId)
        {
            var userId = HttpContext.Items["userId"] as int?;


            var data = await _formAsyncService.GetCurrentTemplateFormAsync(auditId, userId);

            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpGet("getCompletedTemplate/{auditHistoryId}")]
        public async Task<IActionResult> GetCompletedTemplate(int? auditHistoryId)
        {
            var userId = HttpContext.Items["userId"] as int?;

            var data = await _formAsyncService.GetCompletedTemplateFormAsync(auditHistoryId, userId);

            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpGet("getHeader/{templateId}")]
        public async Task<IActionResult> GetHeader(int? templateId)
        {
            var userId = HttpContext.Items["userId"] as int?;


            var data = await _formAsyncService.GetTemplateHeaderAsync(templateId, userId);

            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }


        [AllowAnonymous]
        [HttpPost("createTemplate")]
        public async Task<IActionResult> CreateTemplate(CreateTemplateModel[]? template)
        {
            if (template == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            var userId = HttpContext.Items["userId"] as int?;

            var result = await _formAsyncService.CreateTemplateAsync(template, userId);
            return StatusCode(StatusCodes.Status201Created, JSONSerialization.ConvertToJSON(result));
        }

        [AllowAnonymous]
        [HttpPost("createHeader")]
        public async Task<IActionResult> CreateHeader(HeaderTemplate header)
        {
            if (header == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            var userId = HttpContext.Items["userId"] as int?;

            var result = await _formAsyncService.CreateTemplateHeaderAsync(header, userId);
            return StatusCode(StatusCodes.Status201Created, JSONSerialization.ConvertToJSON(result));
        }
    }
}
