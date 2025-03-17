using CyberSphere.BLL.DTO.LessonDTO;
using CyberSphere.BLL.Services.Interface;
using CyberSphere.DAL.Entities;
using CyberSphere.DAL.Repo.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CyberSphere.PLL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService lessonService;

        public LessonController(ILessonService lessonService)
        {
            this.lessonService = lessonService;
        }
        [HttpGet("get-all-course-lessons")]
        public IActionResult GetLessonsByCourseId(int courseId)
        {
            if(ModelState.IsValid)
            {
                var lessons = lessonService.GetLessonsByCourseId(courseId);
                return Ok(lessons);
            }
            return BadRequest("can not show all lesson .. plz check your code");
        }
        [HttpGet("get-lesson_by-id")]
        public IActionResult GetLessonByID(int id)
        {
            if(ModelState.IsValid)
            {
              var lesson =  lessonService.GetLessonById(id);
                if(lesson == null)
                    return NotFound("not exist");  
                return Ok(lesson);
            }
            return BadRequest("error in model state ....");    
        }
        [HttpPost("add-lesson")]
        public IActionResult AddLesson(CreateLessonDTO creareLessonDTo)
        {
            if (ModelState.IsValid)
            {
                var createdlesson = lessonService.CreateLesson(creareLessonDTo);
                return Ok(createdlesson);

            }
            return BadRequest("can not be to create lesson");
        }
        [HttpPut("{id}")]
        public IActionResult UpdateLesson(int id, [FromForm] UpdateLessonDTO lessonDTO)
        {
            if (ModelState.IsValid)
            {
                var oldlesson = lessonService.GetLessonById(id);
                if (oldlesson != null)
                {
                    var lesson = lessonService.UpdateLesson(id, lessonDTO);
                    return Ok(lesson);

                }
                return BadRequest("this lesson not exist");
            }
            return BadRequest("this lesson not exist");
        }



        [HttpDelete]
        public IActionResult DeleteLesson(int id)
        {
            if(ModelState.IsValid)
            {
                lessonService.DeleteLesson(id);
                return Ok($"the lesson deleted successfully");
            }
            return BadRequest("cant not delete the lesson");
        }


    }
}
