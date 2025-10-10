using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    //Code first approach - what we declare here is going to be applied in the database

    //Entity Framework Core - this provides us with a number advantages:
    //                        1) we can use LINQ
    //                        2) we can make use of LazyLoading - meaning we can view data from
    //                           child tables/parent tables without having to code JOIN statements

    public class Book
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public int CategoryFK { get; set; }
        
        [ForeignKey("CategoryFK")]
        public virtual Category Category { get; set; } //navigational property 

    }
}
