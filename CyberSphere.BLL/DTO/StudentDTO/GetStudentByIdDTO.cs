using CyberSphere.DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.DTO.StudentDTO
{
    public class GetStudentByIdDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string phone { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string UniversityName { get; set; }
        public string? ProfilePictureURL { get; set; }


    }
}
