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
        [DataType(DataType.PhoneNumber)]
        public string phone { get; set; }

        public string Address { get; set; }
        public string UniversityName { get; set; }
        public string? ProfilePictureURL { get; set; }
        public IFormFile? ImageFile { get; set; }
        public ApplicationUser User { get; set; }
    }
}
