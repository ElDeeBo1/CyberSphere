using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.DTO.ArticleDTO
{
    public class UpdateArticleDTO
    {

        public string? Title { get; set; }

        public string? Description { get; set; }
        public string? AuthorName { get; set; }

        public string? Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? PublishedAt { get; set; }
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
