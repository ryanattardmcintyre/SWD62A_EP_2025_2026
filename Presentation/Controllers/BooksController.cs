using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

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
        public IActionResult Create([FromServices] CategoriesRepository categoriesRepository)
        {
            BooksCreateViewModel myModel = new BooksCreateViewModel(categoriesRepository);
            return View(myModel);
        }

        [HttpPost] //Handles the submission form

        public IActionResult Create(BooksCreateViewModel b)
        {
            //TempData survives a redirection
            try
            {
                _booksRepository.Add(b.Book);
                TempData["success"] = "Book added successfully";
                ModelState.Clear();

                return RedirectToAction("Create"); //to redirect the user to the GET create method
            }
            catch (Exception ex)
            {
                TempData["failure"] = "Book failed to be added. Try again later";
                return View(b);
            }
        }

        [HttpGet]
        public IActionResult Update(int id, [FromServices] CategoriesRepository categoriesRepository)
        {
            var originalBook = _booksRepository.Get(id);

            BooksCreateViewModel myModel = new BooksCreateViewModel(categoriesRepository);

            myModel.Book = originalBook; //we need to show existent details of the current book
            return View(myModel);
        }
        [HttpPost]
        public IActionResult Update(BooksCreateViewModel b)
        {
            try
            {
                _booksRepository.Update(b.Book);
                TempData["success"] = "Book updated successfully";
                ModelState.Clear();

                var myparams = new { id = b.Book.Id };

                return RedirectToAction("Update", myparams); //to redirect the user to the GET create method
            }
            catch (Exception ex)
            {
                TempData["failure"] = "Book failed to be updated. Try again later";
                return View(b);
            }

        }


        
    }
}
