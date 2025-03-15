using AutoMapper;
using CyberSphere.BLL.DTO.ArticleDTO;
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
    public class LessonService : ILessonService
    {
        private readonly ILessonRepo lessonRepo;
        private readonly IMapper mapper;

        public LessonService(ILessonRepo lessonRepo,IMapper mapper)
        {
            this.lessonRepo = lessonRepo;
            this.mapper = mapper;
        }

        public CreateLessonDTO CreateLesson(CreateLessonDTO lessondto)
        {
            try
            {
           var lessonentity=  mapper.Map<Lesson>(lessondto);
           var createdentity= lessonRepo.CreateLesson(lessonentity);
            var savedentity = lessonRepo.GetLessonById(createdentity.Id);
            return mapper.Map<CreateLessonDTO>(createdentity);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteLesson(int id)
        {
            try
            {

            var lesson = lessonRepo.GetLessonById(id);
            if (lesson != null)
            {
                lessonRepo.DeleteLesson(lesson);
                return true;
            }
                Console.WriteLine(  "not exist .. check lesson service");
            return false;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public GetLessonByIdDTO GetLessonById(int id)
        {
            var video = lessonRepo.GetLessonById(id);
            return mapper.Map<GetLessonByIdDTO>(video);
        }

        public async Task<IEnumerable<GetAllLessonsByCourseIdDTO>> GetLessonsByCourseId(int courseId)
        {
            var lessons = lessonRepo.GetLessonsByCourseId(courseId);
            return    mapper.Map<IEnumerable<GetAllLessonsByCourseIdDTO>>(lessons);

        }

        public UpdateLessonDTO UpdateLesson(int id, UpdateLessonDTO lesson)
        {
            var existinglesson = lessonRepo.GetLessonById(id);
            if (existinglesson == null)
            {
                throw new Exception("Lesson not found");
            }
            if(! string.IsNullOrEmpty(lesson.Title))
                existinglesson.Title = lesson.Title;
            if(! string.IsNullOrEmpty(lesson.Description))  
                existinglesson.Description = lesson.Description;
            if(! string.IsNullOrEmpty(lesson.VideoURL)) 
                existinglesson.VideoURL = lesson.VideoURL;
            if (lesson.Order != 0)
                existinglesson.Order = lesson.Order;
            if (lesson.Duration != 0)
                existinglesson.Duration = lesson.Duration;

            var updatedLesson = lessonRepo.UpdateLesson(id, existinglesson);
            return mapper.Map<UpdateLessonDTO>(updatedLesson);
        }
    }
}
