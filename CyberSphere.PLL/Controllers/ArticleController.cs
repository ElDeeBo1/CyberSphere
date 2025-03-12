using CyberSphere.BLL.DTO.ArticleDTO;
using CyberSphere.BLL.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CyberSphere.PLL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService articleService;

        public ArticleController(IArticleService articleService)
        {
            this.articleService = articleService;
        }
        [HttpPost("create-article")]
        public IActionResult CreateArticle([FromForm] CreateArticleDTO articleDTO)
        {
            if (ModelState.IsValid)

            {
                var article = articleService.CreateArticle(articleDTO);
                return Ok(article);

            }
            //ModelState.AddModelError("", "cannot create article");
            return BadRequest("cannot create article");

        }
        [HttpGet("get-article-id/{id:int}")]
        public IActionResult GetArticleByID(int id)
        {
            if (ModelState.IsValid)
            {
                var article = articleService.GetArticleById(id);
                if (article != null)
                    return Ok(article);
                return BadRequest("tht article not exists ");

            }
            return BadRequest("  Erroe!!!! ");
        }
        [HttpGet("get-all-articles")]
        public IActionResult GetAllArticles()
        {
            if (ModelState.IsValid)
            {
                var articles = articleService.GetAllArticles();
                return Ok(articles);
            }
            return BadRequest("ther are no articles to show");
        }
        [HttpPut("{id:int}")]     
        //[Consumes("multipart/form-data")]
        public IActionResult UpdateArticle(int id, [FromForm] UpdateArticleDTO updateArticleDTO)
        {
            if (ModelState.IsValid)
            {
                //if (updateArticleDTO.Id == id)
                //{
                    var updated = articleService.UpdateArticle(id,updateArticleDTO);
                    return Ok(updated);
                //}

            //return BadRequest("Error updated");
            }
            return BadRequest("!!!!!!!!!!Error!!!!!!");
        }
        [HttpDelete]
        public IActionResult DeleteArticle(int id)
        {
            if(ModelState.IsValid)
            {
                articleService.DeleteArticle(id);
                return Ok("The article is deleted successfully");
            }
            return BadRequest("error occured when deleting");
        }
    }
}
