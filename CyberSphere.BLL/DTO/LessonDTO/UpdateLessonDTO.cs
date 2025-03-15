using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.DTO.LessonDTO
{
    public class UpdateLessonDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Url)]
        public string VideoURL { get; set; }
        [DataType(DataType.Duration)]

        public int Duration { get; set; }
        public int Order { get; set; }
    }
}
