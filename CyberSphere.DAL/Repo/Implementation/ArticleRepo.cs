using CyberSphere.DAL.Database;
using CyberSphere.DAL.Entities;
using CyberSphere.DAL.Repo.Interface;


namespace CyberSphere.DAL.Repo.Implementation
{
    public class ArticleRepo : IArticleRepo
    {
        private readonly ApplicationDbContext dbContext;

        public ArticleRepo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Article Create(Article article)
        {
            try
            {
                dbContext.Articles.Add(article);
                dbContext.SaveChanges();
                return article;
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Error in Create method!!: {ex.Message}");
                throw;

            }
        }

        public bool Delete(Article article)
        {
            try
            {
                 var data = dbContext.Articles.Find( article.Id);
                if ( data != null )
                {
                dbContext.Articles.Remove(data);
                dbContext.SaveChanges();
                return true;

                }
                Console.WriteLine(  "There is not articles to delete");
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public List<Article> GetAllArticles()
        {
            return dbContext.Articles.ToList();
        }

        public Article GetArticleById(int id)
        {
            return dbContext.Articles.FirstOrDefault(x => x.Id == id);

        }

        public Article Update(int id, Article article)
        {
            try
            {
                var oldarticle = dbContext.Articles.FirstOrDefault(x => x.Id == id);
                oldarticle.AuthorName = article.AuthorName;
                oldarticle.Title = article.Title;
                oldarticle.Description = article.Description;
                oldarticle.PublishedAt = article.PublishedAt;
                oldarticle.Content = oldarticle.Content;
                oldarticle.ImgURL = article.ImgURL;
                dbContext.SaveChanges();
                return article;
            }
            catch (Exception)
            {
                Console.WriteLine("Error in update !");
                throw;
            }
        }
    }
}
