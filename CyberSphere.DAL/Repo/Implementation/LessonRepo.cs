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
    public class LessonRepo : ILessonRepo
    {
        private readonly ApplicationDbContext dbContext;

        public LessonRepo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Lesson CreateLesson(Lesson lesson)
        {
            try
            {
                dbContext.Lessons.Add(lesson);
                dbContext.SaveChanges();
                return lesson;
            }
            catch (Exception)
            {
                Console.WriteLine("error when create video .. plz check repo");

                throw;
            }

        }

        public bool DeleteLesson(Lesson lesson)
        {
            try
            {
                var video = dbContext.Lessons.FirstOrDefault(x => x.Id == lesson.Id);
                if (video != null)
                {
                    dbContext.Lessons.Remove(lesson);
                    dbContext.SaveChanges();
                    return true;
                }
                Console.WriteLine("the lesson not exists");
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Lesson GetLessonById(int id)
        {
            return dbContext.Lessons.FirstOrDefault(x => x.Id == id);
        }

        public List<Lesson> GetLessonsByCourseId(int courseId)
        {
            return dbContext.Lessons
        .Where(l => l.CourseId == courseId)
        .OrderBy(l => l.Order)
        .ToList();
        }

        public Lesson UpdateLesson(int id, Lesson lesson)
        {
            try
            {
                var existingLesson = dbContext.Lessons.FirstOrDefault(x => x.Id == id);
                if (existingLesson == null)
                {
                    throw new Exception("Lesson not found!");
                }
                existingLesson.Title = lesson.Title;
                existingLesson.Description = lesson.Description;
                existingLesson.VideoURL = lesson.VideoURL;
                existingLesson.Order = lesson.Order;
                existingLesson.Duration = lesson.Duration;


                dbContext.SaveChanges();
                return existingLesson;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when updating video: " + ex.Message);
                throw;
            }


        }
    }
}
