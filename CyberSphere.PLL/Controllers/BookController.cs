using CyberSphere.BLL.DTO.BookDTO;
using CyberSphere.BLL.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CyberSphere.PLL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }
        [HttpGet("get-all-books")]
        public IActionResult GetAllBooks()
        {
            if(ModelState.IsValid)
            {
                var books = bookService.GetAllBooks();
                return Ok(books);
            }
            return BadRequest("ca not be found");
        }
        [HttpGet("get-book-by-id")]
        public IActionResult GetBookById(int id)
        {
            if (ModelState.IsValid)
            {
                var book = bookService.GetBookById(id);
                if (book == null)
                    return NotFound("the not not exists");
                return Ok(book);
            }
            return BadRequest("theb error occuered");
        }
        [HttpPost("add-book")]
        public IActionResult AddBook(CreateBookDTO bookDTO)
        {
            if (ModelState.IsValid)
            {
                var book = bookService.CreateBook(bookDTO);
                return Ok(book);
            }
            return BadRequest("can not to be add book");
        }
        [HttpDelete]
        public IActionResult DeleteBook(int id)
        {
            if (ModelState.IsValid)
            {
                bookService.DeleteBook(id);
                return Ok("the book deleted successfully");
            }
            return BadRequest("can not to be able to delete book");
        }
        [HttpPut("{id:int}")]
        public IActionResult UpdateBook(int id,[FromForm]UpdateBookDTO bookDTO)
        {
            if(ModelState.IsValid)
            {
                var existedbook = bookService.GetBookById(id);
                if (existedbook == null)
                    return NotFound("the book not exists");
                var updatedbook = bookService.UpdateBook(id, bookDTO);
                return Ok(updatedbook);
            }
            return BadRequest("error when update");
        }
    }
}
