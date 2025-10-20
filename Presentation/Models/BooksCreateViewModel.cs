using DataAccess.Repositories;
using Domain.Models;

namespace Presentation.Models
{
    //When you need a model ONLY to meet some demand of the UI
    //we create ViewModels
    //Reason: This will only be used by a view to interact with a controller
    //        NOT TO BE USED TO INTERACT BETWEEN CONTROLLER AND DATACCES/DB

    //Reason: A view model usually its a combination of things
    public class BooksCreateViewModel
    {
        public BooksCreateViewModel() { }
        public BooksCreateViewModel(CategoriesRepository categoriesRepository) {
            Categories = categoriesRepository.Get().ToList();
        }
        public List<Category> Categories { get; set; }
        public Book Book { get; set; }

//        public string Message { get; set; } 
    }
}
