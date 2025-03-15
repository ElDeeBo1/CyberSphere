using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.DTO.LessonDTO
{
    public class GetAllLessonsByCourseIdDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoURL { get; set; }
        public int Duration { get; set; }
        public int Order { get; set; }
        public int CourseId { get; set; }
    }
}
