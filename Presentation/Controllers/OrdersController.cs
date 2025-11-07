using DataAccess.Repositories;
using DataAccess.Services;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class OrdersController : Controller
    {
        private OrdersRepository _ordersRepository;
        private IPromotion _promotion;
        private BooksRepository _booksRepository;
        public OrdersController(IPromotion promotion, OrdersRepository ordersRepository, BooksRepository booksRepository) 
        {
            _promotion = promotion;
            _ordersRepository = ordersRepository;
            _booksRepository = booksRepository;
        }
        public IActionResult Buy(List<OrderItem> booksBeingBought)
        {
            _ordersRepository.Checkout("", booksBeingBought, _booksRepository);
            double resultingTotal = _promotion.ApplyPromotion(booksBeingBought);
            //process payment with the resulting total
            TempData["success"] = "Total : " + resultingTotal 
                + " was charged to your visa/mastercard. Books order placed!";

            return RedirectToAction("Index", "Books");
        }
    }
}
