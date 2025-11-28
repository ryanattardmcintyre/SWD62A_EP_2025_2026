using DataAccess.Repositories;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class BackupController : Controller
    {
        public IActionResult Backup(
            [FromKeyedServices("db")] IBooksRepository dbRepo,
            [FromKeyedServices("file")] IBooksRepository fileRepo
            )
        {

            var list = dbRepo.Get();

            BooksFileRepository myActualRepo = (BooksFileRepository) fileRepo;
            myActualRepo.ResetBackupSource();

            foreach (var book in list)
            {
                fileRepo.Add(book);
            }

            //------------------------------------------------

            var loadedBackupCheck = fileRepo.Get();

            string result = $"{loadedBackupCheck.Count()} books were backed up on {DateTime.Now.ToString()}!";

            return Content(result);
        }
    }
}
