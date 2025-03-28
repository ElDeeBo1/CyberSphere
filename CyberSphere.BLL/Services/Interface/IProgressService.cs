using CyberSphere.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Interface
{
    public interface IProgressService
    {
        Task<List<Progress_ModelDTO>> GetStudentProgress(int studentId);
    }
}
