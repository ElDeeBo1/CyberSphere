using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Entities
{
    public class Level
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
        public int? ParentLevelId { get; set; }  // Nullable for top-level levels
        public virtual Level ParentLevel { get; set; }
        public virtual ICollection<Level> ?SubLevels { get; set; } = new List<Level>();
    }
}
