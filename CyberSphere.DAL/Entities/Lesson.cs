using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Entities
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description  { get; set; }
        public string VideoURL  { get; set; }
        public int Duration  { get; set; }
        public int Order  { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public List<Progress> Progresss { get; set; }


    }
}
