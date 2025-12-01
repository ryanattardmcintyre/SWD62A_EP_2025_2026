using DataAccess.Context;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class JournalsRepository
    {
        public ShoppingCartDbContext _context { get; set; }
        //when you have a paramter in the constructor it can be injected using DI
        public JournalsRepository(ShoppingCartDbContext context)
        {
            _context = context;
        }
        public void Add(Journal j)
        {
            _context.Journals.Add(j);
            _context.SaveChanges();
            
        }
    }
}
