using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.DTO.AccountDTO
{
    public class ChangePasswordDTO
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
