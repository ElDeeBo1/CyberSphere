﻿using CyberSphere.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Repo.Interface
{
    public interface IProgressRepo
    {
     
        Task<Progress> GetProgress(int studentId, int courseId);
        Task<List<Progress>> GetStudentCourseLessonsProgress(int studentId, int courseId);
        Task UpdateProgress(int studentId, int courseId, int lessonId, double completionPercentage);
        Task<List<Progress>> GetStudentCoursesProgress(int studentId);
        Task<List<Lesson>> GetCourseLessons(int courseId);

        Task ForceCompleteCourseAsync(int studentId, int courseId);


    }
}
