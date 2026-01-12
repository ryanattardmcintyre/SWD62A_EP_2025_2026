using System.Diagnostics;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Domain.Models;

namespace DataAccess.Repositories
{
    //Repository classes will contain CRUD operations - linq based to communicate with the database
    //This class:
    //- follows the DECORATOR design pattern
    //- it calls the base Repository class to execute the actual checkout
    //- ALSO it tops up the base method with some logging instructions
    public class OrdersLoggingRepository: IOrdersRepository
    {
        private readonly ILogger<OrdersLoggingRepository> _logger; //Change OrdersRepository to OrdersLoggingRepository
        private readonly IOrdersRepository _baseOrdersRepository; //Change from OrdersRepository to IOrdersRepository
        public OrdersLoggingRepository(ILogger<OrdersLoggingRepository> logger, //<2nd change as above
            IOrdersRepository ordersRepository) //Change from OrdersRepository to IOrdersRepository
        {
            _logger = logger;
            _baseOrdersRepository = ordersRepository;
        }

        public void Checkout(string username, List<OrderItem> booksBeingBought, IBooksRepository booksRepo)
        {
            try
            {
                _logger.LogInformation($"User checking out: {username}");

                foreach (var book in booksBeingBought)
                {
                    _logger.LogInformation($"Book Id BEING BOUGHT: {book.BookFK} - Quantity: {book.Qty}");
                }

                _baseOrdersRepository.Checkout(username, booksBeingBought, booksRepo);

                _logger.LogInformation("Order complete!");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking out by " + username);
                throw; //exception is forwarded into the controller because the controller ultimately decides
                        //what to do e.g. load a particular error page? OR display an inline message in the
                        //source view
            }
        }
    }
}
