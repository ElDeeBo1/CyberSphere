using CyberSphere.BLL.DTO.LessonDTO;
using CyberSphere.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Interface
{
    public interface ILessonService
    {
        Task<IEnumerable<GetAllLessonsByCourseIdDTO>> GetLessonsByCourseId(int courseId);
        GetLessonByIdDTO GetLessonById(int id);
        UpdateLessonDTO UpdateLesson(int id,UpdateLessonDTO lesson);
        CreateLessonDTO CreateLesson(CreateLessonDTO lesson);
        bool DeleteLesson(int id);
    }
}
