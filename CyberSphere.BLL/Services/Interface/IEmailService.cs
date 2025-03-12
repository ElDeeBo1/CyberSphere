using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Interface
{
    public interface IEmailService:IEmailSender
    {
            Task SendEmail(string email, string subject, string message);
        
    }
}
