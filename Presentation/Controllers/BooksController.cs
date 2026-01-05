using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.ActionFilter;
using Presentation.Models;
using System.Web;

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


    [Authorize]
    public class BooksController : Controller
    {
        private BooksRepository _booksRepository;
        private OrdersRepository _ordersRepository;
        private IPromotion _promotion;
        private ILogger<BooksController> _logger;
        public BooksController([FromKeyedServices("db")] IBooksRepository booksRepository, OrdersRepository ordersRepository,
            IPromotion promotion, ILogger<BooksController> logger) {
             _booksRepository = (BooksRepository) booksRepository;
            _ordersRepository = ordersRepository;
            _promotion = promotion;
            _logger = logger;
        }

        [HttpGet] //Loads the page (with empty input controls)
        public IActionResult Create([FromServices] CategoriesRepository categoriesRepository)
        {
            BooksCreateViewModel myModel = new BooksCreateViewModel(categoriesRepository);
            return View(myModel);
        }

        [HttpPost] //Handles the submission form
        //Framework service IWebHostEnvironment
        //Application service BooksRepository, CategoriesRepository, ShoppingCartDbContext
        public IActionResult Create(BooksCreateViewModel b, [FromServices]IWebHostEnvironment host, [FromServices] CategoriesRepository categoriesRepository)
        {
            //TempData survives a redirection
            string filename = "";
            try
            {
                _logger.LogInformation("Create method started");

                //cleaning the data related to the uploaded file
                //i.e. a) to give a filename to the file b) where to store the file?
                if (b.UploadedFile != null)
                {

                    filename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(b.UploadedFile.FileName);

                    _logger.LogInformation("Filename generated: " + filename);

                    //absolute path //C:\Users\attar\source\repos\SWD62A_EP_2025_2026\Presentation\wwwroot\images
                    string absolutePath = host.WebRootPath + @"\images\" + filename;

                    _logger.LogWarning("Saving the uploaded file ");
                    if (b.UploadedFile != null)
                    {
                        _logger.LogInformation("File name uploaded: " + b.UploadedFile.FileName +
                            "| File type " + b.UploadedFile.ContentType +
                            "| File size: " + b.UploadedFile.Length);
                    }
                    else
                    {
                        _logger.LogCritical("No file has been uploaded");
                    }

                    using (var myStream = new System.IO.FileStream(absolutePath, FileMode.CreateNew))
                    {
                        b.UploadedFile.CopyTo(myStream);
                    }
                    _logger.LogInformation($"Created {filename} from the uploaded file: {b.UploadedFile.FileName}");

                    b.Book.Path = "\\images\\" + filename; //relative path
                }
                else
                {
                    b.Book.Path = "";
                }
                    _booksRepository.Add(b.Book);

                _logger.LogInformation($"{b.Book.Title} saved to database");

                TempData["success"] = "Book added successfully";
                ModelState.Clear();

                return RedirectToAction("Create"); //to redirect the user to the GET create method
            }
            catch (Exception ex)
            {
                _logger.LogError($"Book with guid {filename} uploaded on {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} " +
                    $"generated this error: " +
                    ex.Message);

                if(ex.InnerException != null)
                {
                    _logger.LogError($"Book with guid {filename} uploaded on {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} " +
                  $"generated this inner exception error: " +
                  ex.InnerException.Message);
                }

                TempData["failure"] = "Book failed to be added. Try again later";
                //return RedirectToAction("Create");
                //retrieve the categories list and pass it to the page
                BooksCreateViewModel myModel = new BooksCreateViewModel(categoriesRepository);
                return View(myModel);
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
        public IActionResult Update(BooksCreateViewModel b, [FromServices] IWebHostEnvironment host)
        {
            try
            {
                var originalBook = _booksRepository.Get(b.Book.Id);

                if (b.UploadedFile != null)
                {
                    //Uploading
                    string filename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(b.UploadedFile.FileName);

                    //absolute path //C:\Users\attar\source\repos\SWD62A_EP_2025_2026\Presentation\wwwroot\images
                    string absolutePath = host.WebRootPath + @"\images\" + filename;

                    using (var myStream = new System.IO.FileStream(absolutePath, FileMode.CreateNew))
                    {
                        b.UploadedFile.CopyTo(myStream);
                    }

                    b.Book.Path = "\\images\\" + filename; //relative path

                    //Deleting the unused image
                    if(System.IO.File.Exists(host.WebRootPath+originalBook.Path))
                    {
                        System.IO.File.Delete(host.WebRootPath + originalBook.Path);
                    }
                }

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

        [ServiceFilter(typeof(QueryActionFilter))]
        [AllowAnonymous]
        public IActionResult Index(string? q, int page = 1, int pageSize = 12)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 12;

            IQueryable<Book> query = string.IsNullOrWhiteSpace(q)
                ? _booksRepository.Get()
                : _booksRepository.Get(q);

            var total = query.Count();

            // stable order for consistent paging
            query = query.OrderBy(b => b.Title).ThenBy(b => b.Id);

            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling(total / (double)pageSize);

            ViewBag.Q = q;
            ViewBag.ResultCount = total;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.FirstItem = total == 0 ? 0 : ((page - 1) * pageSize) + 1;
            ViewBag.LastItem = Math.Min(page * pageSize, total);

            return View(items);
        }


        public IActionResult Details (int id)
        {
            var book = _booksRepository.Get(id);
            if (book == null) {
                TempData["failure"] = "Book id not found";
                return RedirectToAction("Index");
                    }

            return View(book);
        }


        //parameter:
        //if you're targeting a single item then int id would do
        //if you're targeting a number of items then an array of the same type have to be used e.g. int[] ids

        
        private IActionResult Delete(int[] ids)
        {
            foreach (var id in ids) {
                _booksRepository.Delete(id);
                    }
            TempData["success"] = "Books deleted";
            return RedirectToAction("Index");
        }

        private IActionResult Buy(List<OrderItem> booksBeingBought)
        {
            _ordersRepository.Checkout("", booksBeingBought, _booksRepository);
            double resultingTotal = _promotion.ApplyPromotion(booksBeingBought);
            
            //process payment with the resulting total

            TempData["success"] = "Total : " + resultingTotal
                + " was charged to your visa/mastercard. Books order placed!";

            return RedirectToAction("Index", "Books");
        }

        public IActionResult Execute(string todo, int[] ids, int[] quantities , int[] allIds)
        {
            if(todo.ToLower() == "checkout")
            {
                List<OrderItem> items = new List<OrderItem>();
                for (int i = 0; i < ids.Length; i++)
                {
                    var indexOfEvaluatedBookId = allIds.ToList().IndexOf(ids[i]);
                    int qtyOfEvaluatedBookid = quantities[indexOfEvaluatedBookId];
                    if (qtyOfEvaluatedBookid > 0)
                    {
                        items.Add(new OrderItem()
                        {
                            BookFK = ids[i],
                            Qty = qtyOfEvaluatedBookid
                        });
                    }
                }

                return Buy(items);
            }
            else if(todo.ToLower() == "delete")
            {
                return Delete(ids);

            }

            return RedirectToAction("Index");
        }
    }
}
