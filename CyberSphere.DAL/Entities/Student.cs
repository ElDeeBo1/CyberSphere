using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ProfilePictureURL { get; set; }
        public int Age { get; set; }
        public ApplicationUser User { get; set; }   
    }
}
