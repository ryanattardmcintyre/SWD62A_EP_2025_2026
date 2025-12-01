using DataAccess.Repositories;
using DataAccess.Services;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.Factory;
using System.IO.Compression;

namespace Presentation.Controllers
{
    public class BulkImportController : Controller
    {

        public BulkImportController()
        {

        }

        public ActionResult BulkImport([FromKeyedServices("db")] IBooksRepository booksRepo,
            [FromServices] JournalsRepository journalsRepo)
        {

            BookFactory f = new BookFactory((BooksRepository) booksRepo, journalsRepo);
            //Testing method

            string json = @"{
                              ""Title"": ""International Journal of Data Science"",
                              ""Price"": 49.99,
                              ""Stock"": 120,
                              ""CategoryFK"": 1,
                              ""Path"": ""/journals/data-science/vol12-issue3.pdf"",
                              ""Volume"": 1,
                              ""IssueNo"" : 1
                               
                            }";


            f.AddInBulkInDb(json);

            return Content("done");

        }

    }

}
