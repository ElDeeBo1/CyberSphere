using CyberSphere.BLL.DTO;
using CyberSphere.BLL.Services.Interface;
using CyberSphere.DAL.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Implenentation
{
    public class ProgressService : IProgressService
    {
        private readonly IProgressRepo progressRepo;

        public ProgressService(IProgressRepo progressRepo)
        {
            this.progressRepo = progressRepo;
        }
        public async Task<List<Progress_ModelDTO>> GetStudentProgress(int studentId)
        {
            var progress = await progressRepo.GetStudentCoursesProgress(studentId); // استخدام الـ Repo لاسترجاع البيانات

            var progressDtoList = progress.Select(p => new Progress_ModelDTO
            {
                CourseTitle = p.Course.Title,
                CompletionPercentage = p.CompletionPercentage,
                IsCompleted = p.CompletionPercentage >= 100 // الكورس مكتمل عند 100%
            }).ToList();

            return progressDtoList;
        }
    }
}
