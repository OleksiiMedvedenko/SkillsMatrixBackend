using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using Tools.FileServer;
using Tools.Serialization;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace CompetencyMatrixAPI.Controllers
{
    [System.Web.Http.Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        [System.Web.Http.AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.HttpGet("getFile")]
        public IActionResult GetFile()
        {
            var data = ComputerFileProvider.GetFileByPath("M:\\IT\\Restricted\\### Users ###\\OME\\");
            return StatusCode(StatusCodes.Status200OK, JSONSerialization.ConvertToJSON(data));
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("uploadFile")]
        public IActionResult UploadFile()
        {
            try
            {
                // Check if the request contains multipart/form-data
                if (!Request.HasFormContentType)
                {
                    return BadRequest("Invalid request format. Must be multipart/form-data.");
                }

                // Set the directory where the uploaded files will be saved
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

                // Create the directory if it doesn't exist
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Get the uploaded file
                var file = Request.Form.Files[0];
                var fileName = file.FileName;

                // Generate a unique file name to avoid conflicts
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);

                // Move the uploaded file to the final destination
                var filePath = Path.Combine(uploadPath, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Return the full path of the uploaded file
                return Ok(filePath);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during file upload
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
