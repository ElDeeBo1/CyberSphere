using AutoMapper;
using CyberSphere.BLL.DTO.CourseDTO;
using CyberSphere.BLL.DTO.LessonDTO;
using CyberSphere.BLL.Services.Interface;
using CyberSphere.DAL.Entities;
using CyberSphere.DAL.Repo.Implementation;
using CyberSphere.DAL.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Implenentation
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepo courseRepo;
        private readonly IMapper mapper;

        public CourseService(ICourseRepo courseRepo,IMapper mapper)
        {
            this.courseRepo = courseRepo;
            this.mapper = mapper;
        }

        public CreateCourseDTO CreateCourse(CreateCourseDTO createCourseDTO)
        {
            try
            {
                var entity = mapper.Map<Course>(createCourseDTO);
                var created = courseRepo.AddCourse(entity);
                var showed = courseRepo.GetCourse(created.Id);  
                return mapper.Map<CreateCourseDTO>(created);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public UpdateCourseDTO UpdateCourse(int id,UpdateCourseDTO updateCourseDTO)
        {
            var existedcourse = courseRepo.GetCourse(id);
            if(existedcourse == null)
            {
                throw new Exception("course not found");
            }
            if (!string.IsNullOrEmpty(existedcourse.Title))
                existedcourse.Title = updateCourseDTO.Title;
            if (!string.IsNullOrEmpty(existedcourse.Description))
                existedcourse.Description = updateCourseDTO.Description;
            if(updateCourseDTO.LevelId.HasValue)
                existedcourse.LevelId = updateCourseDTO.LevelId.Value;
            var updated = courseRepo.UpdateCourse(id, existedcourse);
            return mapper.Map<UpdateCourseDTO>(updated);

        }

       

        bool ICourseService.DeleteCourse(int id)
        {
            try
            {
                var existedcourse = courseRepo.GetCourse(id);
                if (existedcourse == null)
                    throw new Exception("the course not exists");
                courseRepo.DeleteCourse(existedcourse);

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        List<GetAllCoursesDTO> ICourseService.GetAllCourses()
        {
            var courses = courseRepo.GetAllCourses();
            var result = mapper.Map <List<GetAllCoursesDTO>>(courses); 
            return result.ToList();
        }

        GetCourseByIdDTO ICourseService.GetCourseById(int id)
        {
            var course = courseRepo.GetCourse(id);
            return mapper.Map<GetCourseByIdDTO>(course);
        }


    }
}
