using AutoMapper;
using CyberSphere.BLL.DTO.StudentDTO;
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

        public AddStudentDTO AddStudent(AddStudentDTO student)
        {
            try
            {
                if(student.ImageFile != null)
                {
                    student.ProfilePictureURL= Helper.FileHelper.UploadFile("Student_Img",student.ImageFile);
                }
                var entity = Mapper.Map<Student>(student);
                entity.ProfilePictureURL = student.ProfilePictureURL;
                var created = studentRepo.AddStudent(entity);
                var showed = studentRepo.GetStudentById(created.Id);
                return Mapper.Map<AddStudentDTO>(created);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteStudent(int id)
        {
            try
            {
                    var existed = studentRepo.GetStudentById(id);
                if(existed == null)
                {  return false; }
                studentRepo.DeleteStudent(existed);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List <GetAllStudentsDTO> GetAllStudents()
        {
            try
            {
                 var students = studentRepo.GetAllStudents().ToList();
                return Mapper.Map<List<GetAllStudentsDTO>>(students);   
            }
            catch (Exception)
            {

                throw;
            }
        }

        public GetStudentByIdDTO GetStudentById(int id)
        {
            var student = studentRepo.GetStudentById(id);
            return Mapper.Map<GetStudentByIdDTO>(student);
        }

        public UpdateStudentDTO UpdateStudent(int id, UpdateStudentDTO student)
        {
            var existedstudent = studentRepo.GetStudentById(id);
            if (existedstudent == null)
            {
                throw new Exception("can not be update");
            }
            if (!string.IsNullOrEmpty(student.User.Email))
                existedstudent.User.Email = student.User.Email;
            if (!string.IsNullOrEmpty(student.User.PhoneNumber))
                existedstudent.User.PhoneNumber = student.User.PhoneNumber;
            if (!string.IsNullOrEmpty(student.User.UserName))
                existedstudent.User.UserName = student.User.UserName;

            if (!string.IsNullOrEmpty(student.FirstName))
                existedstudent.FirstName = student.FirstName;

            if (!string.IsNullOrEmpty(student.LastName))
                existedstudent.LastName = student.LastName;
            if (!string.IsNullOrEmpty(student.UniversityName))
                existedstudent.UniversityName = student.UniversityName;
            if (!string.IsNullOrEmpty(student.Address))
                existedstudent.Address = student.Address;
            if (!string.IsNullOrEmpty(student.ProfilePictureURL))
                existedstudent.ProfilePictureURL = student.ProfilePictureURL;
            var updated = studentRepo.UpdateStudent(id, existedstudent);
            return Mapper.Map<UpdateStudentDTO>(updated);
        }
    }
}
