using CyberSphere.BLL.DTO.CertificateDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Interface
{
     public interface ICertificateService
    {
        Task CheckAndGenerateCertificate(int studentid,int courseid);
        Task<List<Certificate_ModelDTO>> GetStudentCertificates(int studentId);
    }
}
