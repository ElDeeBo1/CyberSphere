using CyberSphere.BLL.DTO;
using CyberSphere.DAL.Entities;
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
     
        Task ForceCompleteCourseAsync(int studentId, int courseId);


    }
}
