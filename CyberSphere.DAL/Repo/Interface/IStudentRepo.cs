using CyberSphere.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Repo.Interface
{
    public interface IStudentRepo
    {
       Task <Student> AddStudent(Student student);    
        Task<Student> UpdateStudent(int id,Student student);
       Task <Student> GetStudentById(int id);
      Task < List<Student>> GetAllStudents();
       Task <bool> DeleteStudent(Student student);


    }
}
