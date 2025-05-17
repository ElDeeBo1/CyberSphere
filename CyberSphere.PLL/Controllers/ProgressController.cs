using CyberSphere.BLL.DTO;
using CyberSphere.BLL.Services.Implenentation;
using CyberSphere.BLL.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CyberSphere.PLL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService progressService;

        public ProgressController(IProgressService progressService)
        {
            this.progressService = progressService;
        }
        [Authorize]
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<List<Progress_ModelDTO>>> GetStudentProgress(int studentId)
        {
            var progress = await progressService.GetStudentProgress(studentId);
            if (progress == null || !progress.Any())
            {
                return NotFound("No progress found for this student.");
            }
            return Ok(progress);
        }

        [HttpPost("complete-course")]
        public async Task<IActionResult> CompleteCourse([FromQuery] int studentId, [FromQuery] int courseId)
        {
            await progressService.ForceCompleteCourseAsync(studentId, courseId);
            return Ok(new { message = "Course marked as completed successfully." });
        }
    }

}
