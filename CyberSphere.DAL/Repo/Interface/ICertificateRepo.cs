using CyberSphere.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Repo.Interface
{
    public interface ICertificateRepo
    {
        Task<Certificate> CreateCertificate(Certificate certificate);
        Task<List<Certificate>> GetCertificatesByStudentId(int Studentid);
        Task<bool> CertificateExists(int studentId, int courseId);
       
    }
}
