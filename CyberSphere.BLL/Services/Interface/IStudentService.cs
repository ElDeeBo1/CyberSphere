using CyberSphere.BLL.DTO.StudentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Interface
{
    public interface IStudentService
    {
        AddStudentDTO AddStudent(AddStudentDTO student);
        UpdateStudentDTO UpdateStudent(int id,UpdateStudentDTO student);
       List  <GetAllStudentsDTO> GetAllStudents();
        GetStudentByIdDTO GetStudentById(int id);
        bool DeleteStudent (int id);    
    }
}
