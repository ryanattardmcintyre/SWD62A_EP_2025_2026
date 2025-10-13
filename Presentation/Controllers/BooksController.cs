using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    //Controllers: actual business logic
    //             logic that manages the data 
    //             logic that validates the data
    //             computations 
    //             managing views/pages - passing data to and from pages
    //             managing the User data
    //             error handling
    //             redirections 
    //             management of files
    //             access management
    public class BooksController : Controller
    {
        
        //Contructor Injection (one of the ways how you can apply DI)
        private BooksRepository _booksRepository;
        public BooksController(BooksRepository booksRepository) {
            _booksRepository = booksRepository;
        }

        [HttpGet] //Loads the page (with empty input controls)
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] //Handles the submission form
        public IActionResult Create(Book b)
        {
            _booksRepository.Add(b);
            return View();
        }
       /* public IActionResult Delete(int id)
        {
            BooksRepository booksRepository = new BooksRepository(null);
        }
        public IActionResult Details(int id)
        {
            BooksRepository booksRepository = new BooksRepository(null);

        }*/
    }
}
