using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.CreateModels;
using Models.Model;
using Newtonsoft.Json;
using Services.ServiceAsync;
using Services.ServiceAsync.Interface;
using Tools.Serialization;

namespace CompetencyMatrixAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    //[Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeAsyncService _employeeAsyncService;

        public EmployeeController(IEmployeeAsyncService employeeAsyncService)
        {
            _employeeAsyncService = employeeAsyncService;
        }

        [AllowAnonymous]
        [HttpGet("getEmployee/{employeeId}")]
        public async Task<IActionResult> GetEmployee(int? employeeId)
        {
            var data = await _employeeAsyncService.GetEmployeeAsync(employeeId, HttpContext.Items["userId"] as int?);

            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpGet("getEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            var data = await _employeeAsyncService.GetEmployeesAsync(HttpContext.Items["userId"] as int?);

            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpPost("createEmployee")]
        public async Task<IActionResult> CreateEmployeePost(EmployeeCreateModel? employee)
        {
            if (employee == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            var result = await _employeeAsyncService.CreateEmployeeAsync(employee, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status201Created, JSONSerialization.ConvertToJSON(result));
        }

        [AllowAnonymous]
        [HttpPost("updateEmployee")]
        public async Task<IActionResult> UpdateEmployee(UpdateEmpoyeeModel? employee)
        {
            if (employee == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            var result = await _employeeAsyncService.UpdateEmployeeAsync(employee, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status201Created, JSONSerialization.ConvertToJSON(result));
        }

        [AllowAnonymous]
        [HttpPut("deactivateEmployee/{employeeId}")]
        public async Task<IActionResult> DeactivateEmployee(int? employeeId, int? userId)
        {
            var data = await _employeeAsyncService.DeactivateEmployeeAsync(employeeId, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status202Accepted, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpPut("activateEmployee/{employeeId}")]
        public async Task<IActionResult> ActivateEmployee(int? employeeId, int? userId)
        {
            var data = await _employeeAsyncService.ActivateEmployeeAsync(employeeId, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status202Accepted, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpGet("getEmployeeByDepartment/{departmentId}")]
        public async Task<IActionResult> GetEmployeeByDepartment(int? departmentId)
        {
            var data = await _employeeAsyncService.GetEmployeeByDepartmentAsync(departmentId, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status202Accepted, JSONSerialization.ConvertToJSON(data));
        }
    }
}
