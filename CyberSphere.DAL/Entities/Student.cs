﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePictureURL { get; set; }
        public string Address { get; set; }
        public string About { get; set; }
        public int Age { get; set; }
        public string? PhoneNumber { get; set; } // تمت إضافته هنا
        public string UserId { get; set; } // ربط الطالب بالمستخدم
        public List<Certificate> Certificates { get; set; }
        public List<Progress> Progresss { get; set; }   
        public ApplicationUser User { get; set; }

        public List<Skill> Skills { get; set; } = new();
    }
}
