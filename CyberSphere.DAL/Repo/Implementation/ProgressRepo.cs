using CyberSphere.DAL.Database;
using CyberSphere.DAL.Entities;
using CyberSphere.DAL.Repo.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Repo.Implementation
{
    public class ProgressRepo : IProgressRepo
    {
        private readonly ApplicationDbContext dbContext;

        public ProgressRepo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        //public async Task<Progress> GetProgress(int _studentId, int _courseId)
        //{
        //    return await dbContext.Progresss
        //        .Include(p=>p.Student).Include(p=>p.Course)
        //        .FirstOrDefaultAsync(p => p.StudentId == _studentId && p.CourseId == _courseId);
        //}

        //public void UpdateProgress(Progress progress)
        //{
        //    dbContext.Progresss.Update(progress);
        //    dbContext.SaveChanges();

        //}
        public async Task<List<Progress>> GetStudentCoursesProgress(int studentId)
        {
            return await dbContext.Progresss
                .Where(p => p.StudentId == studentId && p.LessonId == null) // فقط تقدم الكورسات
                .Include(p => p.Course) // جلب بيانات الكورس
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<Progress> GetProgress(int studentId, int courseId)
        {
            return await dbContext.Progresss
                .Where(p => p.StudentId == studentId && p.CourseId == courseId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Progress>> GetStudentCourseLessonsProgress(int studentId, int courseId)
        {
            return await dbContext.Progresss
                .Where(p => p.StudentId == studentId && p.CourseId == courseId)
                .ToListAsync();
        }

        public async Task UpdateProgress(int studentId, int courseId, int lessonId, double completionPercentage)
        {
            var progress = await dbContext.Progresss
                .FirstOrDefaultAsync(p => p.StudentId == studentId && p.CourseId == courseId && p.LessonId == lessonId);

            if (progress == null)
            {
                progress = new Progress
                {
                    StudentId = studentId,
                    CourseId = courseId,
                    LessonId = lessonId,
                    CompletionPercentage = completionPercentage
                };
                await dbContext.Progresss.AddAsync(progress);
            }
            else
            {
                progress.CompletionPercentage = completionPercentage;
            }

            // ✅ حساب نسبة التقدم الإجمالية
            int allLessons = await dbContext.Lessons
                .Where(l => l.CourseId == courseId)
                .CountAsync();

            if (allLessons > 0) // ⬅️ منع القسمة على الصفر
            {
                int completedLessons = await dbContext.Progresss
                    .Where(p => p.StudentId == studentId && p.CourseId == courseId && p.CompletionPercentage >= 100)
                    .CountAsync();

                double overallCompletion = (completedLessons / (double)allLessons) * 100;

                // ✅ تحديث السجل العام لتقدم الكورس
                var courseProgress = await dbContext.Progresss
                    .FirstOrDefaultAsync(p => p.StudentId == studentId && p.CourseId == courseId && p.LessonId == null);

                if (courseProgress == null)
                {
                    courseProgress = new Progress
                    {
                        StudentId = studentId,
                        CourseId = courseId,
                        CompletionPercentage = overallCompletion
                    };
                    await dbContext.Progresss.AddAsync(courseProgress);
                }
                else
                {
                    courseProgress.CompletionPercentage = overallCompletion;
                }
            }

            await dbContext.SaveChangesAsync();
        }


    }
}
