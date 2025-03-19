using CyberSphere.BLL.DTO.CourseDTO;
using CyberSphere.BLL.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CyberSphere.PLL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService courseService;

        public CourseController(ICourseService courseService)
        {
            this.courseService = courseService;
        }
        [HttpGet("get-course")]
        public IActionResult GetCourseById(int id)
        {
            if(ModelState.IsValid)
            {
                var course = courseService.GetCourseById(id);
                if(course == null)
                    Console.WriteLine("not exist");
                return Ok(course);
            }
            return BadRequest("can not view");
        }
        [HttpPost("add-course")]
        public IActionResult CreateCourse(CreateCourseDTO courseDTO)
        {
            if(ModelState.IsValid)
            {
                var course = courseService.CreateCourse(courseDTO);
                return Ok(course);
            }
            return BadRequest("Casnnt add course");
        }
        [HttpGet("gett-all-courses")]
        public IActionResult GetAllCourses()
        {
            if(ModelState.IsValid)
            {
                var courses = courseService.GetAllCourses();
                return Ok(courses);

            }
            return BadRequest("erorr !11111111");
        }
        [HttpDelete]
        public IActionResult DeleteCourse(int id)
        {
            if(ModelState.IsValid)
            {
                courseService.DeleteCourse(id);
                return Ok("course deleted successfully");
            }
            return BadRequest("error when delete");

        }
        [HttpPut("{id}")]
        public IActionResult UpdateCourse(int id,[FromForm]UpdateCourseDTO courseDTO)
        {
            if(ModelState.IsValid)
            {
                var course = courseService.GetCourseById(id);
                if(course != null)
                {
                    var updated = courseService.UpdateCourse(id, courseDTO);    
                    return Ok(updated);
                }
                return BadRequest("the course not exists");
            }
            return BadRequest("error !");
        }
    }
}
