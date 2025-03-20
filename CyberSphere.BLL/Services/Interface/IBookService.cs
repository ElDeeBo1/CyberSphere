using CyberSphere.BLL.DTO.BookDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Interface
{
    public interface IBookService
    {
        CreateBookDTO CreateBook(CreateBookDTO bookDTO);
        UpdateBookDTO UpdateBook(int id,UpdateBookDTO bookDTO);
       List <GetAllBooksDTO> GetAllBooks();
        GetBookByIdDTO GetBookById(int id);
        bool DeleteBook(int id);
    }
}
