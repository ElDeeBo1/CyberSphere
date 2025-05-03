using CyberSphere.DAL.Database;
using CyberSphere.DAL.Entities;
using CyberSphere.DAL.Repo.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Repo.Implementation
{
    public class SkillRepo : ISkillRepo
    {
        private readonly ApplicationDbContext dbContext;

        public SkillRepo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Skill> AddSkill(Skill skill)
        {
            dbContext.Skills.Add(skill);
            await dbContext.SaveChangesAsync();
            return skill;

        }
        public async Task<bool> DeleteSkill(Skill skill)
        {
            var skilled = await dbContext.Skills.FirstOrDefaultAsync(s =>s.Id == skill.Id);
            if (skilled != null)
            {
                dbContext.Skills.Remove(skill);
               await dbContext.SaveChangesAsync();
            }
            return false;
        }

        public Task<List<Skill>> GetAllSkillsByStudentId(int StudentId)
        {
            return dbContext.Skills
                .Where(s =>s.StudentId == StudentId) .ToListAsync();
        }

        public Task<Skill> GetSkill(int id)
        {
            return dbContext.Skills.FirstOrDefaultAsync(s =>s.Id == id);
        }

        public async Task<Skill> UpdateSkill(int id, Skill skill)
        {
            try
            {
                var existedskill = await dbContext.Skills.FirstOrDefaultAsync(s => s.Id == id);
                if (existedskill == null)
                {
                    throw new Exception(" no skills are exist");

                }
                existedskill.Name = skill.Name; 
                await dbContext.SaveChangesAsync();
                return existedskill;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
