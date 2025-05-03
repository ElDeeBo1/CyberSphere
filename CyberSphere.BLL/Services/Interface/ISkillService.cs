using CyberSphere.BLL.DTO.SkillDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Interface
{
    public interface ISkillService
    {
        Task<AddSkillDTO> AddSkill(AddSkillDTO skillDTO);   
        Task<UpdateSkillDTO> UpdateSkill(int id,UpdateSkillDTO skillDTO);
        Task <bool> DeleteSkill(int id);
        Task<GetSkillDTO> GetSkill(int id);
        Task<List<GetSkillDTO>> GetAllSkillsByStudentId (int studentid);
    }
}
