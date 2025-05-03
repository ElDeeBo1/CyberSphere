using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Entities
{
    public class Skill
    {
        public int Id { get; set; }
        public string  Name { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
