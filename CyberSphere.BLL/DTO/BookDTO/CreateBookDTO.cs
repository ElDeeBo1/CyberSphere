﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.DTO.BookDTO
{
    public class CreateBookDTO
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string BookURL { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;

    }
}
