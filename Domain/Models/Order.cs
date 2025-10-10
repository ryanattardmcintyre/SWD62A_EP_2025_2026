using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; } //Guid.NewGuid(); //newid()

        public string Username { get; set; }

        public DateTime DatePlaced { get; set; }

    }
}
