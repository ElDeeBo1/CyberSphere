using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.DTO.AccountDTO
{
    public class SMTPDTO
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EnableSsl { get; set; }
        public string IsBodyHTML { get; set; }
    }
}
