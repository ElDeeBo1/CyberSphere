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
    public class CourseRepo : ICourseRepo
    {
        private readonly ApplicationDbContext dbContext;

        public CourseRepo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Course AddCourse(Course course)
        {
            try
            {
                dbContext.Courses.Add(course);
                dbContext.SaveChanges();
                return course;
            }
            catch (Exception)
            {
                Console.WriteLine("can not add course");
                throw;
            }
        }

        public bool DeleteCourse(Course course)
        {
            try
            {
                var existedcourse = dbContext.Courses.FirstOrDefault(x => x.Id == course.Id);
                if (existedcourse != null)
                {
                    dbContext.Courses.Remove(existedcourse);
                    dbContext.SaveChanges();
                    return true;
                }
                    Console.WriteLine(" the course not exists .. plz check the repo"  );
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Course> GetAllCourses()
        {
            return dbContext.Courses
                .AsNoTracking()
                .Include(c => c.Lessons)
                .Include(c => c.Level)
                .ToList();
        }

        public Course GetCourse(int id)
        {
               return dbContext.Courses
                .Include(c =>c.Level)
                .Include(c =>c.Lessons)
                .FirstOrDefault(x => x.Id == id);

        }

        public Course UpdateCourse(int id, Course course)
        {
            try
            {
            var existedcourse = dbContext.Courses.FirstOrDefault(x =>x.Id == id);   
            if (existedcourse== null)
            {
                throw new Exception("not found");

            }

                if (!string.IsNullOrEmpty(course.Title))
                    existedcourse.Title = course.Title;

                if (!string.IsNullOrEmpty(course.Description))  
                    existedcourse.Description = course.Description;

                if (course.LevelId > 0)  
                    existedcourse.LevelId = course.LevelId;
                dbContext.SaveChanges();
            return existedcourse;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
