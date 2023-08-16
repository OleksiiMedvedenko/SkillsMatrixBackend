using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CreateModels;
using Models.Model;
using Newtonsoft.Json;
using Services.ServiceAsync.Interface;
using Tools.Serialization;

namespace CompetencyMatrixAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IAreasAsyncService _areasAsyncService;

        public AreaController(IAreasAsyncService areasAsyncService)
        {
            _areasAsyncService = areasAsyncService;
        }

        [AllowAnonymous]
        [HttpGet("getAreas")]
        public async Task<IActionResult> GetAreas()
        {
            var data = await _areasAsyncService.GetAreasAsync();
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpPost("createArea")]
        public async Task<IActionResult> CreateAreaPost(AreaCreateModel? area)
        {
            if (area == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            var result = await _areasAsyncService.CreateAreaAsync(area);
            return StatusCode(StatusCodes.Status201Created, JSONSerialization.ConvertToJSON(result));
        }

        [AllowAnonymous]
        [HttpGet("getEmployeeAreas/{employeeId}")]
        public async Task<IActionResult> GetEmployeeAreas(int? employeeId)
        {
            var data = await _areasAsyncService.GetEmployeeAreasAsync(employeeId);
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }
    }
}
