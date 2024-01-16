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
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionAsyncService _questionAsyncService;

        public QuestionController(IQuestionAsyncService questionAsyncService)
        {
            _questionAsyncService = questionAsyncService;
        }

        [AllowAnonymous]
        [HttpGet("getQuestions")]
        public async Task<IActionResult> GetQuestions()
        {
            var userId = HttpContext.Items["userId"] as int?;

            var data = await _questionAsyncService.GetQuestionsAsync(userId);
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [AllowAnonymous]
        [HttpPost("createQuestion")]
        public async Task<IActionResult> CreateAreaPost(Question? question)
        {
            if (question == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
            var userId = HttpContext.Items["userId"] as int?;

            var result = await _questionAsyncService.CreateQuestionAsync(question, userId);
            return StatusCode(StatusCodes.Status201Created, JSONSerialization.ConvertToJSON(result));
        }
    }
}
