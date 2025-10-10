using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    
    //Services - fire-and-forget model i.e. user makes a request, the controller services the request
    //                                      replies back, and forget
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            Message myMessage = new Message();
            myMessage.Text = "Welcome in the test controller";
            myMessage.Author = User.Identity.IsAuthenticated? User.Identity.Name: "Me";

            return View(myMessage);
        }

        public IActionResult SubmitMessage(Message message)
        {
            message.Author = message.Author.ToUpper();
            message.Text = message.Text.Pascalize(); //akdkfaslOIakdfjalsd -> Akdkfasl.....

            return View("Index",message);
        }
    }
}
