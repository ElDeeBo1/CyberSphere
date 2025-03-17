using AutoMapper;
using CyberSphere.BLL.DTO.ArticleDTO;
using CyberSphere.BLL.Helper;
using CyberSphere.BLL.Services.Interface;
using CyberSphere.DAL.Entities;
using CyberSphere.DAL.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Implenentation
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepo articleRepo;
        private readonly IMapper mapper;

        public ArticleService(IArticleRepo articleRepo, IMapper mapper)
        {
            this.articleRepo = articleRepo;
            this.mapper = mapper;
        }
        public CreateArticleDTO CreateArticle(CreateArticleDTO articleDTO)
        {
            if (articleDTO == null)
                throw new ArgumentNullException(nameof(articleDTO), "Article data is null now");

            try
            {
                if (articleDTO.ImageFile != null)
                {
                    articleDTO.Image = FileHelper.UploadFile("Article_Img", articleDTO.ImageFile);

                }
                var articleEntity = mapper.Map<Article>(articleDTO);
                //Console.WriteLine($"قبل الحفظ - ImgURL: {articleEntity.ImgURL}");

                articleEntity.ImgURL = articleDTO.Image; // ✅ تعيين المسار

                var createdEntity = articleRepo.Create(articleEntity);
                //Console.WriteLine($"بعد الحفظ - ImgURL في قاعدة البيانات: {createdEntity.ImgURL}");
                var savedArticle = articleRepo.GetArticleById(createdEntity.Id);
                //Console.WriteLine($"بعد الحفظ - ImgURL في قاعدة البيانات: {savedArticle.ImgURL}");

                return mapper.Map<CreateArticleDTO>(createdEntity);


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when creating: {ex.Message}");
                throw;
            }

        }

        public bool DeleteArticle(int id)
        {
            try
            {
                var article = articleRepo.GetArticleById(id);
                articleRepo.Delete(article);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public List<GetAllArticlesDTO> GetAllArticles()
        {

            var articles = articleRepo.GetAllArticles();
            return mapper.Map<List<GetAllArticlesDTO>>(articles);
        }

        public GetArticleByIdDTO GetArticleById(int id)
        {
            var article = articleRepo.GetArticleById(id);
            return mapper.Map<GetArticleByIdDTO>(article);
        }


        public UpdateArticleDTO UpdateArticle(int id, UpdateArticleDTO articleDTO)
        {
            var existingArticle = articleRepo.GetArticleById(id);
            if (existingArticle == null)
            {
                throw new Exception("Article not found");
            }

  
            if (!string.IsNullOrEmpty(articleDTO.Title))
                existingArticle.Title = articleDTO.Title;

            if (!string.IsNullOrEmpty(articleDTO.Description))
                existingArticle.Description = articleDTO.Description;

            if (!string.IsNullOrEmpty(articleDTO.AuthorName))
                existingArticle.AuthorName = articleDTO.AuthorName;

            if (!string.IsNullOrEmpty(articleDTO.Content))
                existingArticle.Content = articleDTO.Content;

            if (articleDTO.PublishedAt.HasValue)
                existingArticle.PublishedAt = articleDTO.PublishedAt.Value;

            if (articleDTO.ImageFile != null)
            {
                existingArticle.ImgURL = FileHelper.SaveImage(articleDTO.ImageFile);
            }

            var updatedArticle = articleRepo.Update(id, existingArticle);
            return mapper.Map<UpdateArticleDTO>(updatedArticle);
        }

    }
}
