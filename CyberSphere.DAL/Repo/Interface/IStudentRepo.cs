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
        Student AddStudent(Student student);    
        Student UpdateStudent(int id,Student student);
        Student GetStudentById(int id);
        List<Student> GetAllStudents();
        bool DeleteStudent(Student student);


    }
}
