using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.DTO.SkillDTO
{
    public class GetAllSkillsDTO
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }  // Optional
        public List<GetSkillDTO> Skills { get; set; }
    }
}
