using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Entities
{
    public class Progress
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int LessonsCompleted { get; set; }
        public int TotalLessons { get; set; }
        public double ProgressPercentage => (TotalLessons == 0) ? 0 : (LessonsCompleted / (double)TotalLessons) * 100;
        public bool IsCompleted => ProgressPercentage >= 100;

        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }
    }
}
