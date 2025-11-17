using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class NoPromotion : IPromotion
    {
        private BooksRepository _booksRepository;
        public NoPromotion(BooksRepository booksRepository) { 
          _booksRepository = booksRepository;
        }
        public double ApplyPromotion(List<OrderItem> itemsBeingBought)
        {
            double total = 0;
            foreach(var item in itemsBeingBought)
            {
                var originalBook = _booksRepository.Get(item.BookFK);
                total += (originalBook.Price * item.Qty);
            }

            total *= 1.18;

            return total;
        }
    }
}
