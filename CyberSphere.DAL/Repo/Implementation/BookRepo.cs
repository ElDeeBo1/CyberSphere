using CyberSphere.DAL.Database;
using CyberSphere.DAL.Entities;
using CyberSphere.DAL.Migrations;
using CyberSphere.DAL.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.DAL.Repo.Implementation
{
    public class BookRepo : IBookRepo
    {
        private readonly ApplicationDbContext dbContext;

        public BookRepo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Book AddBook(Book book)
        {
            try
            {
                dbContext.Books.Add(book);
                dbContext.SaveChanges();
                return book;    
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteBook(Book book)
        {
            try
            {
                var existedbook = dbContext.Books.FirstOrDefault(b => b.Id == book.Id);
                if (existedbook != null)
                {
                    dbContext.Books.Remove(existedbook);
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Book> GetAllBooks()
        {
            return dbContext.Books.ToList();
        }

        public Book GetById(int id)
        {
            return dbContext.Books.FirstOrDefault(b => b.Id == id);
        }

        public Book UpdateBook(int id,Book book)
        {
            var existedbook = dbContext.Books.FirstOrDefault(b =>b.Id == id);
            if (existedbook == null)
            {
                throw new Exception("this book not found");
            }
            if(!string.IsNullOrEmpty(book.Title))
                existedbook.Title = book.Title;
            if(!string.IsNullOrEmpty(book.Description))
                existedbook.Description = book.Description;
            if(!string.IsNullOrEmpty(book.BookURL))
                existedbook.BookURL = book.BookURL;
            dbContext.SaveChanges();
            return existedbook;
        }
    }
    }
