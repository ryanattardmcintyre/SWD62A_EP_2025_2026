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

        

    
    }
}
