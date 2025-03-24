using AutoMapper;
using CyberSphere.BLL.DTO.StudentDTO;
using CyberSphere.BLL.Helper;
using CyberSphere.BLL.Services.Interface;
using CyberSphere.DAL.Entities;
using CyberSphere.DAL.Repo.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Implenentation
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepo studentRepo;

        public StudentService(IStudentRepo studentRepo, IMapper mapper)
        {
            this.studentRepo = studentRepo;
            Mapper = mapper;
        }

        public IMapper Mapper { get; }

        public async Task <AddStudentDTO> AddStudent(AddStudentDTO student)
        {
            try
            {
                if(student.ImageFile != null)
                {
                    student.ProfilePictureURL= Helper.FileHelper.UploadFile("Student_Img",student.ImageFile);
                }
                var entity = Mapper.Map<Student>(student);
                entity.ProfilePictureURL = student.ProfilePictureURL;
                var created =await studentRepo.AddStudent(entity);
                var showed = studentRepo.GetStudentById(created.Id);
                return Mapper.Map<AddStudentDTO>(created);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task <bool> DeleteStudent(int id)
        {
            try
            {
                    var existed = await studentRepo.GetStudentById(id);
                if(existed == null)
                {  return false; }
               await     studentRepo.DeleteStudent(existed);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task <List <GetAllStudentsDTO>> GetAllStudents()
        {
            try
            {
                 var students = await studentRepo.GetAllStudents();
                return Mapper.Map<List<GetAllStudentsDTO>>(students);   
            }
            catch (Exception)
            {

                throw;
            }
        }

        public  async Task <GetStudentByIdDTO> GetStudentById(int id)
        {
            var student = await studentRepo.GetStudentById(id);
            return Mapper.Map<GetStudentByIdDTO>(student);
        }

        public async Task<UpdateStudentDTO> UpdateStudent(int id, UpdateStudentDTO student)
        {
            var existedstudent = await studentRepo.GetStudentById(id);
            if (existedstudent == null)
            {
                throw new Exception("can not be update");
            }
            if (existedstudent.User == null)
            {
                existedstudent.User = new ApplicationUser();
            }

            if (!string.IsNullOrEmpty(student.Email))
                existedstudent.User.Email = student.Email;
            if (!string.IsNullOrEmpty(student.PhoneNumber))
                existedstudent.PhoneNumber = student.PhoneNumber;
            if (!string.IsNullOrEmpty(student.UserName))
                existedstudent.User.UserName = student.UserName;
            if(student.Age.HasValue)
                existedstudent.Age = student.Age.Value;
            if (!string.IsNullOrEmpty(student.FirstName))
                existedstudent.FirstName = student.FirstName;

            if (!string.IsNullOrEmpty(student.LastName))
                existedstudent.LastName = student.LastName;
            if (!string.IsNullOrEmpty(student.About))
                existedstudent.About = student.About;
            if (!string.IsNullOrEmpty(student.Address))
                existedstudent.Address = student.Address;
            if (student.ImageFile != null)
                
                existedstudent.ProfilePictureURL = Helper.FileHelper.SaveImage(student.ImageFile);
            var updated = await studentRepo.UpdateStudent(id, existedstudent);
            return Mapper.Map<UpdateStudentDTO>(updated);
        }
    }
}
