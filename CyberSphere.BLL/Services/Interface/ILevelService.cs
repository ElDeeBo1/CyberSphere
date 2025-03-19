using CyberSphere.BLL.DTO.CourseDTO;
using CyberSphere.BLL.DTO.LevelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Interface
{
    public interface ILevelService
    {
        List<GetAllLevelsDTO> GetAllLevels();
        GetLevelByIdDTO GetLevelById(int id);
        CreateLevelDTO CreateLevel(CreateLevelDTO levelDTO);
        UpdateLevelDTO UpdateLevel(int id,UpdateLevelDTO levelDTO);
        bool DeleteLevel(int id);
    }
}
