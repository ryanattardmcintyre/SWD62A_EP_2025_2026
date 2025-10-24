using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    //Note: The context class is an abstraction of the database
    //Note 2: every context class has to inherit from DbContext; IdentityDbContext inherits from DbContext
    //        adv of using IdentityDbContext -> AspNetUsers, AspNetRoles, AspNetUserRoles, ...
    public class ShoppingCartDbContext : IdentityDbContext
    {
        public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options)
            : base(options)
        {
            //applies the connectionstring
        }

        public DbSet<Book> Books { get; set; } //these will be our tables
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

    }
}
