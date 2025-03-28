using CyberSphere.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Repo.Interface
{
    public interface IProgressRepo
    {
        //Task<Progress> GetProgress(int _studentId, int _courseId);
        //void UpdateProgress(Progress progress);

        Task<Progress> GetProgress(int studentId, int courseId);
        Task<List<Progress>> GetStudentCourseLessonsProgress(int studentId, int courseId);
        Task UpdateProgress(int studentId, int courseId, int lessonId, double completionPercentage);
    }
}
