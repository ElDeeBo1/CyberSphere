using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Entities
{
    public class Certificate
    {
        public int Id { get; set; }
        public string CertificateURL { get; set; }
        public DateTime IssuedAt { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public int ProgressId { get; set; }
        public virtual Progress Progress { get; set; }
    }
}
