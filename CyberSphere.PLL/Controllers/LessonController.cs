using CyberSphere.BLL.DTO.LessonDTO;
using CyberSphere.BLL.Services.Interface;
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
        [HttpGet]
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
        [HttpGet]
        public async Task IActionResult  <list<GetLessonsByCourseID>>(int courseID)
        {
            if(ModelState.IsValid)
            {
                lessonService.GetLessonsByCourseId(courseID);

            }
        }
        [HttpPost]
        public IActionResult AddLesson(CreateLessonDTO creareLessonDTo)
        {
            if (ModelState.IsValid)
            {
                var createdlesson = lessonService.CreateLesson(creareLessonDTo);
                return Ok(createdlesson);

            }
            return BadRequest("can not be to create lesson");
        }

    }
}
