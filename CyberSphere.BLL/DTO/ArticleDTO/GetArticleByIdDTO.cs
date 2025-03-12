using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.DTO.ArticleDTO
{
    public class GetArticleByIdDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The description is required")]
        public string Description { get; set; }
        public string AuthorName { get; set; }

        [Required(ErrorMessage = "The content is required")]
        public string Content { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime PublishedAt { get; set; } = DateTime.Now;
        public string? Image { get; set; }



    }
}
