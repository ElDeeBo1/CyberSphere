using AutoMapper;
using CyberSphere.BLL.DTO.BookDTO;
using CyberSphere.BLL.Services.Interface;
using CyberSphere.DAL.Entities;
using CyberSphere.DAL.Migrations;
using CyberSphere.DAL.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Implenentation
{
    public class BookService : IBookService
    {
        private readonly IBookRepo bookRepo;
        private readonly IMapper mapper;

        public BookService(IBookRepo bookRepo, IMapper mapper)
        {
            this.bookRepo = bookRepo;
            this.mapper = mapper;
        }
        public CreateBookDTO CreateBook(CreateBookDTO bookDTO)
        {
            var entity = mapper.Map<Book>(bookDTO);
            var created = bookRepo.AddBook(entity);
            var showed = bookRepo.GetById(created.Id);
            return mapper.Map<CreateBookDTO>(created);
        }

        public bool DeleteBook(int id)
        {
            var existedbook = bookRepo.GetById(id);
            if (existedbook != null)
            {
                bookRepo.DeleteBook(existedbook);
                return true;
            }
            return false;
        }

        public List <GetAllBooksDTO> GetAllBooks()
        {
            var books = bookRepo.GetAllBooks().ToList();
            return mapper.Map<List<GetAllBooksDTO>>(books);

        }

        public GetBookByIdDTO GetBookById(int id)
        {
            var book = bookRepo.GetById(id);    
            return mapper.Map<GetBookByIdDTO>(book);
        }

        public UpdateBookDTO UpdateBook(int id,UpdateBookDTO bookDTO)
        {
            var existedbook = bookRepo.GetById(id);
            if (existedbook == null)
            {
                throw new Exception("can not found");
            }
            if (!string.IsNullOrEmpty(bookDTO.Title))
                existedbook.Title = bookDTO.Title;
            if (!string.IsNullOrEmpty(bookDTO.Description))
                existedbook.Description = bookDTO.Description;
            if (!string.IsNullOrEmpty(bookDTO.BookURL))
                existedbook.BookURL = bookDTO.BookURL;
            var shoewd = bookRepo.UpdateBook(id,existedbook);
            return mapper.Map<UpdateBookDTO>(existedbook);
        }
    }
}
