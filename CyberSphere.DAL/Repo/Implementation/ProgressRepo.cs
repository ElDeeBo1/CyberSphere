﻿using CyberSphere.DAL.Database;
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


        public async Task<List<Progress>> GetStudentCoursesProgress(int studentId)
        {
            return await dbContext.Progresss
                .Where(p => p.StudentId == studentId) // Remove the LessonId filter
                .Include(p => p.Course)
                .AsNoTracking()
                .ToListAsync();
        }



        public async Task<Progress> GetProgress(int studentId, int courseId)
        {
            return await dbContext.Progresss
                .Include(p => p.Student)
                    .ThenInclude(s => s.User) // لو بتستخدم User مرتبط بالطالب
                .Include(p => p.Course)
                .FirstOrDefaultAsync(p => p.StudentId == studentId && p.CourseId == courseId);
        }

        public async Task<List<Progress>> GetStudentCourseLessonsProgress(int studentId, int courseId)
        {
            return await dbContext.Progresss
                .Where(p => p.StudentId == studentId && p.CourseId == courseId)
                .ToListAsync();
        }



        public async Task<List<Lesson>> GetCourseLessons(int courseId)
        {
            return await dbContext.Lessons
                .Where(l => l.CourseId == courseId)
                .ToListAsync();
        }
        public async Task UpdateProgress(int studentId, int courseId, int lessonId, double completionPercentage)
        {
            // تحقق من أن الدرس موجود
            if (lessonId != 0)
            {
                var lessonExists = await dbContext.Lessons.AnyAsync(l => l.Id == lessonId && l.CourseId == courseId);
                if (!lessonExists)
                {
                    throw new ArgumentException("The lesson ID is invalid or does not belong to the specified course.");
                }
            }

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

            if (allLessons > 0)
            {
                int completedLessons = await dbContext.Progresss
                    .Where(p => p.StudentId == studentId && p.CourseId == courseId && p.CompletionPercentage >= 100 && p.LessonId != null)
                    .CountAsync();

                double overallCompletion = (completedLessons / (double)allLessons) * 100;

                var courseProgress = await dbContext.Progresss
                    .FirstOrDefaultAsync(p => p.StudentId == studentId && p.CourseId == courseId && p.LessonId == null);

                if (courseProgress == null)
                {
                    courseProgress = new Progress
                    {
                        StudentId = studentId,
                        CourseId = courseId,
                        LessonId = null,
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
        public async Task ForceCompleteCourseAsync(int studentId, int courseId)
        {
            var progress = await dbContext.Progresss
                .FirstOrDefaultAsync(p => p.StudentId == studentId && p.CourseId == courseId && p.LessonId == null);

            if (progress == null)
            {
                progress = new Progress
                {
                    StudentId = studentId,
                    CourseId = courseId,
                    LessonId = null,
                    CompletionPercentage = 100
                };
                await dbContext.Progresss.AddAsync(progress);
            }
            else
            {
                progress.CompletionPercentage = 100;
            }

            await dbContext.SaveChangesAsync();
        }


    }
}
