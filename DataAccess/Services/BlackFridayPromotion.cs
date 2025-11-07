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
    public class BlackFridayPromotion: IPromotion
    {
        private BooksRepository _booksRepository;
        public double DiscountPercentage { get; set; }
        public BlackFridayPromotion(BooksRepository booksRepository, double discountPercentage = 0.2)
        {
            _booksRepository = booksRepository;
            DiscountPercentage = discountPercentage;

            if (DiscountPercentage < 0  || discountPercentage >=1) discountPercentage = 0.2;
        }
        public double ApplyPromotion(List<OrderItem> itemsBeingBought)
        {
            double total = 0;
            foreach (var item in itemsBeingBought)
            {
                var originalBook = _booksRepository.Get(item.Id);
                total += originalBook.Price;
            }

            total *= DiscountPercentage; //applying a discount of a value passed in the constructor

            total *= 1.18; //working out the added tax

            return total;
        }
    }
}
