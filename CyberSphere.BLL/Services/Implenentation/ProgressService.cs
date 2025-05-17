using CyberSphere.BLL.DTO;
using CyberSphere.BLL.Services.Implenentation;
using CyberSphere.BLL.Services.Interface;
using CyberSphere.DAL.Repo.Implementation;
using CyberSphere.DAL.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Implenentation
{
    public class ProgressService : IProgressService
    {
        private readonly IProgressRepo progressRepo;
        private readonly ICertificateService certificateService;

        public ProgressService(IProgressRepo progressRepo,ICertificateService certificateService)
        {
            this.progressRepo = progressRepo;
            this.certificateService = certificateService;
        }
       
        public async Task<List<Progress_ModelDTO>> GetStudentProgress(int studentId)
        {
            var progress = await progressRepo.GetStudentCoursesProgress(studentId);

            if (progress == null || !progress.Any())
            {
                Console.WriteLine("No progress data found for student ID " + studentId);
            }

            var progressDtoList = new List<Progress_ModelDTO>();

            foreach (var p in progress)
            {
                bool isCompleted = p.CompletionPercentage >= 100;

                if (isCompleted)
                {
                 
                    await certificateService.CheckAndGenerateCertificate(p.StudentId, p.CourseId.Value);
                }

                progressDtoList.Add(new Progress_ModelDTO
                {
                    CourseTitle = p.Course.Title,
                    CompletionPercentage = p.CompletionPercentage,
                    IsCompleted = isCompleted
                });
            }

            return progressDtoList;
        }
       
        public async Task ForceCompleteCourseAsync(int studentId, int courseId)
        {
           
            await progressRepo.ForceCompleteCourseAsync(studentId, courseId);

           
            var updatedProgress = await progressRepo.GetProgress(studentId, courseId);

           
            if (updatedProgress != null && updatedProgress.CompletionPercentage >= 100)
            {
                await certificateService.CheckAndGenerateCertificate(studentId, courseId);
            }
        }


    }
}

