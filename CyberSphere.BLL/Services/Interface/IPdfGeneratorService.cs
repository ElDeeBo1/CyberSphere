using CyberSphere.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Interface
{
    public interface IPdfGeneratorService
    {
        Task<string> GenerateCertificate(Student student, Course course);
    }
}
