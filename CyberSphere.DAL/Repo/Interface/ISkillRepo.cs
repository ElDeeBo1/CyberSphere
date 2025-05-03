using CyberSphere.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Repo.Interface
{
    public interface ISkillRepo
    {
        Task<Skill> AddSkill(Skill skill);
        Task<Skill> UpdateSkill(int id,Skill skill);
        Task<bool> DeleteSkill(Skill skill); 
        Task<Skill> GetSkill(int id);
       Task< List<Skill> >GetAllSkillsByStudentId(int StudentId);
    }
}
