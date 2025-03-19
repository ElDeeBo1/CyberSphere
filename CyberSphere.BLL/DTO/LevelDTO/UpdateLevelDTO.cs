using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.DTO.LevelDTO
{
    public class UpdateLevelDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? ParentLevelId { get; set; }
    }
}
