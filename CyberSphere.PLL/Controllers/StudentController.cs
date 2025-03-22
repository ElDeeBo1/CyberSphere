using CyberSphere.BLL.DTO.StudentDTO;
using CyberSphere.BLL.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CyberSphere.PLL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService  studentService)
        {
            this.studentService = studentService;
        }
        [HttpGet("get-all-students")]
        public async Task<IActionResult> GetAllStudents()
        {
            if(ModelState.IsValid)
            {
                var students = await studentService.GetAllStudents();
                return Ok(students);
            }
            return BadRequest("can not be able to show students");
        }
        [HttpGet("get-student-by-id")]
        public  async Task <IActionResult> GetStudentById(int id)
        {
            if(ModelState.IsValid)
            {
                var student= await studentService.GetStudentById(id);
                if(student == null)
                    return NotFound("the student not exists");
                return Ok(student);
            }
            return BadRequest("can not be able to show student");

        }
        [HttpPost("add-student")]
        public async Task <IActionResult> AddStudent(AddStudentDTO studentDTO)
        {
            if(ModelState.IsValid)
            {
                var student = await studentService.AddStudent(studentDTO);
                return Ok(student);
            }
            return BadRequest("can not be able to add student");

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromForm] UpdateStudentDTO studentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var updatedStudent = await studentService.UpdateStudent(id, studentDTO);

            if (updatedStudent == null)
            {
                return NotFound("The student does not exist");
            }

            return Ok(updatedStudent);
        }

        [HttpDelete]
        public async Task <IActionResult >DeleteStudent(int id)
        {
            if (ModelState.IsValid)
            {
               await  studentService.DeleteStudent(id);
                return Ok("the student deleted successfully");
            }
            return BadRequest("can not be able to delete student");

        }
    }
}
