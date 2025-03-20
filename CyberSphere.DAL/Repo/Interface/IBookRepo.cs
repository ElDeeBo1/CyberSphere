using CyberSphere.DAL.Entities;
using CyberSphere.DAL.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Repo.Interface
{
    public interface IBookRepo
    {
        Book AddBook(Book book);    
        Book UpdateBook(int id,Book book);
        Book GetById(int id);
        List<Book> GetAllBooks();
        bool DeleteBook(Book book);
    }
}
