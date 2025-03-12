using CyberSphere.DAL.Entities;


namespace CyberSphere.DAL.Repo.Interface
{
    public interface IArticleRepo
    {
        List<Article>GetAllArticles();
        Article GetArticleById(int id);
        Article Create(Article article);
        Article Update(int id,Article article);
        bool Delete(Article article);
    }
}
