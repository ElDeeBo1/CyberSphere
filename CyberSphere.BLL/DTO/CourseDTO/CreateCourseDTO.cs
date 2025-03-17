using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.DTO.CourseDTO
{
    public class CreateCourseDTO
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public int LevelId { get; set; } 
    }
}
