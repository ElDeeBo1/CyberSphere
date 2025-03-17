using CyberSphere.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Repo.Interface
{
    public interface ICourseRepo
    {
        List<Course> GetAllCourses();
        Course GetCourse(int id);
        Course AddCourse(Course course);
        Course UpdateCourse(int id,Course course); 
        bool DeleteCourse(Course  course);  
    }
}
