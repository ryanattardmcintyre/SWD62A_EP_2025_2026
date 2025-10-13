using DataAccess.Context;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    //Repository classes which contain raw CRUD operations
    //C = create
    //R = read
    //U = update
    //D = delete


    //IQueryable vs IEnumerable/List/ToList....
    //IQueryable - it just forms SQL commands (LINQ (at runtime) -> in SQL) and it stops there
    //IEnumerable - it does what happens in IQueryable and it opens the connection to the database to 
    //              to execute the prepared SQL command

    public class BooksRepository
    {
        public ShoppingCartDbContext _context { get; set; }
        //when you have a paramter in the constructor it can be injected using DI
        public BooksRepository(ShoppingCartDbContext context) {
            _context = context;
        }

        public IQueryable<Book> Get() { 
           return _context.Books.OrderBy(x=>x.Title);
        }

        public Book Get(int id) {
            return Get().SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<Book> Get(string keyword)
        {
            return Get().Where(x => x.Title.Contains(keyword));
        }

        public void Add(Book book) { 
          _context.Books.Add(book); //this saves the data in the (volatile) memory
          _context.SaveChanges(); //this what actually pushes the data into the database (the persistance data layer)
        }

        public void Update(Book book) {
            var originalBook = Get(book.Id);
            if (originalBook != null)
            {
                originalBook.Title = book.Title;
                originalBook.Price = book.Price;
                originalBook.CategoryFK = book.CategoryFK;
                originalBook.Stock = book.Stock;

                _context.SaveChanges();
            }
        }

        public void Delete(Book b) { 
            _context.Books.Remove(b);
            _context.SaveChanges();
        }

        public void Delete(int id) {
            var originalBook = Get(id);
            if (originalBook != null) 
                    Delete(originalBook);
        }


    }
}
