using CyberSphere.BLL.DTO.CourseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.DTO.LevelDTO
{
    public class GetLevelByIdDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ParentLevelId { get; set; }
        public List<Course_ModelDTO> Courses { get; set; }
        public List<GetLevelByIdDTO> SubLevels { get; set; }
    }
}
