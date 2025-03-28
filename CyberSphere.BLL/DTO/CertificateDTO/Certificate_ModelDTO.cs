using CyberSphere.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.DTO.CertificateDTO
{
    public class Certificate_ModelDTO
    {
        public string CourseTitle { get; set; }
        public DateTime IssuedAt { get; set; }
        public string CertificateURL { get; set; }
    }
}
