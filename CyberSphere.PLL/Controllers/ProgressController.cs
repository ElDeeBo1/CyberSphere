using CyberSphere.BLL.DTO;
using CyberSphere.BLL.Services.Interface;
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
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<List<Progress_ModelDTO>>> GetStudentProgress(int studentId)
        {
            var progress = await progressService.GetStudentProgress(studentId);
            return Ok(progress);
        }
    }
}
