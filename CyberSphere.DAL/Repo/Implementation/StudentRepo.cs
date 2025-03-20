using CyberSphere.DAL.Database;
using CyberSphere.DAL.Entities;
using CyberSphere.DAL.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Repo.Implementation
{
    public class StudentRepo : IStudentRepo
    {
        private readonly ApplicationDbContext dbContext;

        public StudentRepo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Student AddStudent(Student student)
        {
            try
            {
                dbContext.Students.Add(student);
                dbContext.SaveChanges();
                return student;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteStudent(Student student)
        {
            try
            {
                var existedstudent = dbContext.Students.FirstOrDefault(s => s.Id == student.Id);
                if (existedstudent != null)
                {
                    dbContext.Students.Remove(existedstudent);
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Student> GetAllStudents()
        {
            return dbContext.Students.ToList();
        }

        public Student GetStudentById(int id)
        {
            return dbContext.Students.FirstOrDefault(s => s.Id == id);
        }

        public Student UpdateStudent(int id, Student student)
        {
            try
            {
                var existedstudent = dbContext.Students.FirstOrDefault(s => s.Id == id);
                if (existedstudent == null)
                {
                    throw new Exception("can not be update");
                }
                if (!string.IsNullOrEmpty(student.User.Email))
                    existedstudent.User.Email = student.User.Email;
                if (!string.IsNullOrEmpty(student.User.PhoneNumber))
                    existedstudent.User.PhoneNumber = student.User.PhoneNumber;
                if(!string.IsNullOrEmpty(student.User.UserName))
                    existedstudent.User.UserName = student.User.UserName;
                if(!string.IsNullOrEmpty(student.FirstName))
                    existedstudent.FirstName = student.FirstName;

                if(!string.IsNullOrEmpty(student.LastName))
                    existedstudent.LastName = student.LastName;      
                if(!string.IsNullOrEmpty(student.UniversityName))
                    existedstudent.UniversityName = student.UniversityName;
                if(!string.IsNullOrEmpty(student.Address))
                    existedstudent.Address = student.Address;
                if(!string.IsNullOrEmpty(student.ProfilePictureURL))
                    existedstudent.ProfilePictureURL = student.ProfilePictureURL;
                dbContext.SaveChanges();
                return existedstudent;




            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
