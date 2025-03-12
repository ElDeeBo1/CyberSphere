using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.DTO.AccountDTO
{
    public class RegisterDTO
    {
        [Required]
        public string UserName { get; set; }
        //public int Age { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Password and ConfirmPassword doesnot match")]
        public string ConfirmPassword { get; set; }
    }
}
