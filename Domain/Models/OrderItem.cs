using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class OrderItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int BookFK { get; set; }
        [ForeignKey("BookFK")]
        public virtual Book Book { get; set; }

        public int Qty { get; set; }

        public Guid OrderFK { get; set; }
        [ForeignKey("OrderFK")]
        public virtual Order Order { get; set; }
    }
}
