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
    public class StudentRepo : IStudentRepo
    {
        private readonly ApplicationDbContext dbContext;

        public StudentRepo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task <Student> AddStudent(Student student)
        {
            try
            {
                dbContext.Students.Add(student);
              await  dbContext.SaveChangesAsync();
                return student;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task <bool> DeleteStudent(Student student)
        {
            try
            {
                var existedstudent = await dbContext.Students.Include(s =>s.User).FirstOrDefaultAsync(s => s.Id == student.Id);
                if (existedstudent != null)
                {
                    dbContext.Students.Remove(existedstudent);
                    dbContext.SaveChanges();
                     return  true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task <List<Student>> GetAllStudents()
        {
            return await dbContext.Students.Include(u =>u.User).ToListAsync();
        }

        public async Task <Student> GetStudentById(int id)
        {
            return await dbContext.Students.Include(u =>u.User).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task <Student> UpdateStudent(int id, Student student)
        {
            try
            {
                var existedstudent = await dbContext.Students
      .Include(s => s.User)
      .FirstOrDefaultAsync(s => s.Id == id);

                if (existedstudent == null)
                {
                    throw new Exception("can not be update");
                }

                if (!string.IsNullOrEmpty(student.User.PhoneNumber))
                    existedstudent.PhoneNumber = student.PhoneNumber;
      

                if (!string.IsNullOrEmpty(student.User.Email))
                    existedstudent.User.Email = student.User.Email;
                //if (!string.IsNullOrEmpty(student.User.PhoneNumber))
                //    existedstudent.User.PhoneNumber = student.User.PhoneNumber;
                if (!string.IsNullOrEmpty(student.User.UserName))
                    existedstudent.User.UserName = student.User.UserName;
                existedstudent.Age = student.Age;
                if (!string.IsNullOrEmpty(student.FirstName))
                    existedstudent.FirstName = student.FirstName;
                if (!string.IsNullOrEmpty(student.LastName))
                    existedstudent.LastName = student.LastName;
                if (!string.IsNullOrEmpty(student.About))
                    existedstudent.About = student.About;
                if (!string.IsNullOrEmpty(student.Address))
                    existedstudent.Address = student.Address;
                if (!string.IsNullOrEmpty(student.ProfilePictureURL))
                    existedstudent.ProfilePictureURL = student.ProfilePictureURL;

                await dbContext.SaveChangesAsync();
                return existedstudent;


            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Student?> GetStudentByUserId(string userId)
        {
            return await dbContext.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.User.Id == userId);
        }
    }
}
