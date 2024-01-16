using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Newtonsoft.Json;
using Services.ServiceAsync.Interface;
using Tools.Serialization;

namespace CompetencyMatrixAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DepatmentController : ControllerBase
    {
        private readonly IDepartmentAsyncService _departmentAsyncService;

        public DepatmentController(IDepartmentAsyncService departmentAsyncService)
        {
            _departmentAsyncService = departmentAsyncService;
        }

        [AllowAnonymous]
        [HttpGet("getDepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            var data = await _departmentAsyncService.GetDepartmentsAsync(HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpPost("createDepartment/{area}")]
        public async Task<IActionResult> CreateDepartment(string? department)
        {
            if (department == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            var data = JsonConvert.DeserializeObject<Department>(department);

            var result = await _departmentAsyncService.CreateDepartmentAsync(data, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status201Created, JSONSerialization.ConvertToJSON(result));
        }
    }
}
