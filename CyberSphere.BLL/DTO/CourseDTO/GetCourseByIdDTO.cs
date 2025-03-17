using CyberSphere.BLL.DTO.LessonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.DTO.CourseDTO
{
    public class GetCourseByIdDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int LevelId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LevelName { get; set; } 
        public List<Lesson_ModelDTO> Lessons { get; set; }

    }
}
