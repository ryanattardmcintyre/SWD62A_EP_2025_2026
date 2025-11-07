using DataAccess.Context;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class OrdersRepository
    {
        public ShoppingCartDbContext _context { get; set; }
        public OrdersRepository(ShoppingCartDbContext context)
        {
            _context = context;
        }

        private void AddOrder(Order o)
        { 
            _context.Orders.Add(o);
            _context.SaveChanges();
        }

        private void AddOrderItem(OrderItem o)
        {
            _context.OrderItems.Add(o);
            _context.SaveChanges();
        }

        //Entry point to the above two methods
        public void Checkout (string username, List<OrderItem> booksBeingBought, BooksRepository booksRepo)
        {
            Order o = new Order();
            o.Id = Guid.NewGuid(); //43168B8A-E0BC-4BDC-9670-783EC47D51AF
            o.DatePlaced = DateTime.Now;
            o.Username = username;
            AddOrder(o); //adds the order in the db

            foreach (var item in booksBeingBought)
            {
                item.OrderFK = o.Id; //setting the individual orderItems with the earlier order id
                var originalBook = booksRepo.Get(item.BookFK);
                if (originalBook != null)
                {
                    if(originalBook.Stock >= item.Qty)//do we have enough stock to sell?
                    {
                        originalBook.Stock -= item.Qty;
                        booksRepo.Update(originalBook);

                        AddOrderItem(item); //adds the orderitem(s) in the OrderItems table
                    }
                }

            }

        }


    }
}
