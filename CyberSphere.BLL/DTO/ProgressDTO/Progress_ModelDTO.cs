using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.DTO
{
    public class Progress_ModelDTO
    {
        public string CourseTitle { get; set; }
        public double CompletionPercentage { get; set; }
        public bool IsCompleted { get; set; }
    }
}
