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

    /*
     * 
     * Method Injection:
     *  [FromServices] BooksRepository _booksRepository //as a parameter in the method
     *  
     * Property Injection:
     * BooksController books = new BooksController();
        books._testRepository = app.Services.GetRequiredService<TestRepository>(); 
    //after you declare _testRepository as a property
     * 
     */
    public class BooksController : Controller
    {
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
            //TempData survives a redirection

            try
            {
                _booksRepository.Add(b);
                TempData["success"] = "Book added successfully";
                ModelState.Clear();
              //  ViewBag.success = "Book added successfully"; //this doesn't survive a redirection
                return View();
            }
            catch (Exception ex)
            {
                TempData["failure"] = "Book failed to be added. Try again later";
                return View(b);
            }
        }
       /* public IActionResult Delete(int id)
        {
        
        }
        public IActionResult Details(int id)
        {
           

        }*/
    }
}
