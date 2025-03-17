using CyberSphere.BLL.DTO.CourseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Interface
{
    public interface ICourseService
    {
        List<GetAllCoursesDTO> GetAllCourses();
        CreateCourseDTO CreateCourse(CreateCourseDTO createCourseDTO);
        UpdateCourseDTO UpdateCourse(int id,UpdateCourseDTO updateCourseDTO);
        GetCourseByIdDTO GetCourseById(int id);
        bool DeleteCourse(int id);
    }
}
