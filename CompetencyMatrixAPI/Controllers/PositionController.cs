using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CreateModels;
using Services.ServiceAsync.Interface;
using Tools.Serialization;

namespace CompetencyMatrixAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PositionController : ControllerBase
    {
        private readonly IPositionAsyncService _positionAsyncService;

        public PositionController(IPositionAsyncService positionAsyncService)
        {
            _positionAsyncService = positionAsyncService;
        }

        [AllowAnonymous]
        [HttpGet("getPositionsByArea/{areaId}")]
        public async Task<IActionResult> GetPositionsByArea(int? areaId)
        {
            var data = await _positionAsyncService.GetPositionsByAreaAsync(areaId, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpGet("getPositions")]
        public async Task<IActionResult> GetPositions()
        {
            var data = await _positionAsyncService.GetPositionsAsync(HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpPost("createPosition")]
        public async Task<IActionResult> CreatePositionPost(PositionCreateModel? position)
        {
            if (position == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            var result = await _positionAsyncService.CreatePositionAsync(position, HttpContext.Items["userId"] as int?);
            return StatusCode(StatusCodes.Status201Created, JSONSerialization.ConvertToJSON(result));
        }
    }
}
