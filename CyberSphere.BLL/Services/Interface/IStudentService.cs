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
       Task <AddStudentDTO> AddStudent(AddStudentDTO student);
        Task <UpdateStudentDTO> UpdateStudent(int id,UpdateStudentDTO student);
      Task < List  <GetAllStudentsDTO>> GetAllStudents();
       Task <GetStudentByIdDTO> GetStudentById(int id);
       Task <bool> DeleteStudent (int id);    
    }
}
