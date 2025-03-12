using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.DTO.AccountDTO
{
    public class TwoFactorDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Provider { get; set; }
        public string Token { get; set; }
    }
}
