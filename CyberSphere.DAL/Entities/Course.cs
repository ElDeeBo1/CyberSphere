using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAT { get; set; }
        public virtual List <Lesson> Lessons { get; set; }
        public int LevelId { get; set; }
        public virtual Level Level { get; set; }
        public List<Progress> Progresss { get; set; }

    }
}
