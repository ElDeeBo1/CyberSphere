using CyberSphere.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Repo.Interface
{
    public interface ILessonRepo
    {

        //Task<IEnumerable<Lesson>> GetLessonsByCourseId(int courseId);
        //Task<Lesson> GetLessonByIdAsync(int id);
        //Task AddLessonAsync(Lesson lesson);
        //Task UpdateLessonAsync(Lesson lesson);
        //Task DeleteLessonAsync(int id);

        List<Lesson> GetLessonsByCourseId(int courseId);
        Lesson GetLessonById(int id);
        Lesson UpdateLesson (int id,Lesson lesson);    
        Lesson CreateLesson (Lesson lesson);
        bool DeleteLesson(Lesson lesson);

    }
}
