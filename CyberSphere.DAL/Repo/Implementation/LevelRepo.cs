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
    public class LevelRepo : ILevelRepo
    {
        private readonly ApplicationDbContext dbContext;

        public LevelRepo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Level Addlevel(Level level)
        {
            dbContext.Levels.Add(level);
            dbContext.SaveChanges();
            return level;
        }

        public bool DeleteLevel(Level leveled)
        {
            var level = dbContext.Levels.FirstOrDefault(i => i.Id == leveled.Id);
            if(level != null)
            {
                dbContext.Levels.Remove(level);
                dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public Level GetLevel(int id)
        {
            return dbContext.Levels
                .Include(l => l.SubLevels)
                .Include(l => l.Courses)
                .FirstOrDefault(c => c.Id == id);
        }

        public List<Level> GetLevels()
        {
            return dbContext.Levels
                .Include(l => l.SubLevels)
                .Include(l => l.Courses)
                .ToList();  
        }

        public Level Updatelevel(int id,Level level)
        {
            var existedlevel = dbContext.Levels.FirstOrDefault(l => l.Id == id);
            if (existedlevel == null)
                throw new Exception("not exist");
            existedlevel.Title = level.Title;
            existedlevel.Description = level.Description;
            existedlevel.SubLevels = level.SubLevels;
            existedlevel.Courses = level.Courses;
            existedlevel.ParentLevel = level.ParentLevel;
            dbContext.SaveChanges();
            return existedlevel;
        }
    }
}
