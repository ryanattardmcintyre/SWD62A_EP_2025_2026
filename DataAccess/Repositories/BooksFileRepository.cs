using Castle.Core.Configuration;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class BooksFileRepository : IBooksRepository
    {
        private string filePath;
        public BooksFileRepository(string path ) {
            filePath = path;
        }

        public void ResetBackupSource()
        {
            if(System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }

        public void Add(Book book)
        {
            string json = JsonConvert.SerializeObject(book);
            System.IO.File.AppendAllLines(filePath, new List<string>() { json });
        }

        public IQueryable<Book> Get()
        {
            string [] contents = System.IO.File.ReadAllLines(filePath); 

            List<Book> allBooks = new List<Book>();

            foreach(string line in contents)
            {
                allBooks.Add( JsonConvert.DeserializeObject<Book>(line));
            }

            return allBooks.AsQueryable();

        }
    }
}
