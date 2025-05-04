
using CyberSphere.BLL.DTO.CertificateDTO;
using CyberSphere.BLL.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CyberSphere.PLL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateService certificateService;

        public CertificateController(ICertificateService certificateService)
        {
            this.certificateService = certificateService;
        }
        [Authorize]
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<List<Certificate_ModelDTO>>> GetStudentCertificates(int studentId)
        {
            var certificates = await certificateService.GetStudentCertificates(studentId);
            return Ok(certificates);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateCertificate([FromBody] CertificateRequestDTO request)
        {
            await certificateService.CheckAndGenerateCertificate(request.StudentId, request.CourseId);
            return Ok(new { message = "Certificate processed successfully" });
        }
    }


    public class CertificateRequestDTO
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }

}
