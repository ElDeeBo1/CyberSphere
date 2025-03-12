using CyberSphere.BLL.DTO.ArticleDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Interface
{
    public interface IArticleService
    {
        CreateArticleDTO CreateArticle(CreateArticleDTO articleDTO);
        GetArticleByIdDTO GetArticleById(int id);
       List< GetAllArticlesDTO> GetAllArticles();
        UpdateArticleDTO UpdateArticle(int id,UpdateArticleDTO articleDTO);
        bool DeleteArticle(int id); 

        
    }
}
