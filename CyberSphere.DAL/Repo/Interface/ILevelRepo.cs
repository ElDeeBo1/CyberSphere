using CyberSphere.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Repo.Interface
{
    public interface ILevelRepo
    {
        Level GetLevel(int id);
        List<Level> GetLevels();
        Level Addlevel(Level level);
        Level Updatelevel(int id,Level level);
        bool DeleteLevel(int id);
    }
}
