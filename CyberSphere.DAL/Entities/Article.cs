using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public string? ImgURL { get; set; }
        public string Content { get; set; }
        public DateTime PublishedAt { get; set; }=DateTime.UtcNow;
    }
}
