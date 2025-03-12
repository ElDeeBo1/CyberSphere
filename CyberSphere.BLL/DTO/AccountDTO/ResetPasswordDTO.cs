using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.DTO.AccountDTO
{
    public class ResetPasswordDTO
    {

        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string token { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string newPassword { get; set; }
        [Required]
        [Compare("newPassword", ErrorMessage = "New password and confirm new password doesnot match")]
        public string ConfirmPassword { get; set; }
    }
}
