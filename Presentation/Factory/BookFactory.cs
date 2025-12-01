using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Presentation.Factory
{
    public class BookFactory
    {
        private BooksRepository _booksRepo;
        private JournalsRepository _journalsRepo;
        public BookFactory(BooksRepository booksRepo, JournalsRepository journalsRepo) { 
         _booksRepo = booksRepo;
            _journalsRepo = journalsRepo;
        }

        /// <summary>
        /// a method that is going to add items in the database
        /// depending on the type
        /// </summary>
        /// <param name="json"></param>
        public void AddInBulkInDb(string json)
        {
            var item = Build(json);
            if (item.GetType() == typeof(Book))
            {
                _booksRepo.Add((Book)item);
            }
            else
            {
                _journalsRepo.Add((Journal)item);
            }
        }

        /// <summary>
        /// method is going to build the right object based on the type
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IItem Build(string json)
        {
            dynamic dynamicObject = JsonConvert.DeserializeObject(json);
            IItem i;
            if(dynamicObject.Volume != null)
            {
                i = JsonConvert.DeserializeObject<Journal>(json);
            }
            else
            {
                i = JsonConvert.DeserializeObject<Book>(json);
            }

            return i;
        }
    }
}
