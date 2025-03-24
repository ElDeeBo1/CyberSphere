using CyberSphere.DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.DTO.StudentDTO
{
    public class AddStudentDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public string Address { get; set; }
        public string About { get; set; }
        public string? ProfilePictureURL { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }

        public string? PhoneNumber { get; set; } // تمت إضافته هنا
        public string UserId { get; set; } // ربط الطالب بالمستخدم
    }
}
