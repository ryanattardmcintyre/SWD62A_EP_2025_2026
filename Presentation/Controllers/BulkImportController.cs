using Microsoft.AspNetCore.Mvc;
using Presentation.Factory;
using System.IO.Compression;

namespace Presentation.Controllers
{
    public class BulkImportController : Controller
    {
        /*  public IActionResult Approve(string type, int restaurant)
          {
              List<IItemValidating> items = new List<IItemValidating>();
              if(type=="restaurant")
              {
                  //fetch the pending restaurants from the db
                  //pass the restaurant data into the view
                  //populate items
              }

              if(type=="menuitem")
              {
                  //fetch the menuitems for the selected restaurant
                  //populate items
              }

              TempData["viewingMode"] = "approval";
              return Catalogue(items);
          }

          public IActionResult Catalogue (List<IItemValidating> items)
          {
              return View("Catalogue", items);
          }

          public IActionResult Index()
          {
              TempData["viewingMode"] = "normal";
          }

          public IActionResult Import(string json, [FromServices] ImportItemFactory factory)
          {
              var obj = factory.Create();
              List<IItemValidating> items = new List<IItemValidating>();

              //you parse the json, 
              //form the objects

              // IMemoryCache to store the items there and showing them to the user
          }

          public IActionResult Commit()
          {
              //load the data from IMemoryCache
              //via the ItemsInMemoryRepository 

              //... you save the images uploaded
              //...you link the images with the respective records

              //save the data together with the images uploaded into the db
              //via the ItemsDbRepository
          }
        */


        //is that when this method is called it generates a zip file
        //containing all the images inside //wwwroot/images
        //and download it
        public IActionResult GenerateZip([FromServices]IWebHostEnvironment _env)
        {
            //access the ItemsMemoryCacheRepository
            //load the items from there

            //for every item it has to generate this zip file


            var imagesRoot = Path.Combine(_env.WebRootPath, "images");

            if (!Directory.Exists(imagesRoot))
            {
                return NotFound("Images folder not found.");
            }

            using var ms = new MemoryStream();
            using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                foreach (var filePath in Directory.GetFiles(imagesRoot, "*", SearchOption.TopDirectoryOnly))
                {
                    var fileName = Path.GetFileName(filePath);                 // e.g. banner1.jpg
                    var folderName = Path.GetFileNameWithoutExtension(fileName); // e.g. banner1

                    // images/banner1/banner1.jpg
                    var entryName = Path.Combine("images", folderName, fileName)
                                    .Replace("\\", "/");

                    var entry = zip.CreateEntry(entryName, CompressionLevel.Optimal);

                    using var entryStream = entry.Open();
                    using var fileStream = System.IO.File.OpenRead(filePath);
                    fileStream.CopyTo(entryStream);
                }
            }

            var fileNameZip = $"images-{DateTime.UtcNow:yyyyMMddHHmmss}.zip";
            var bytes = ms.ToArray();

            return File(bytes, "application/zip", fileNameZip);
        }

    }
}
