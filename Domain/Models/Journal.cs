using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Journal: IItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public int CategoryFK { get; set; }

        [ForeignKey("CategoryFK")]
        public virtual Category Category { get; set; } //navigational property 

        public string Path { get; set; }

        public int Volume { get; set; }
        public int IssueNo { get; set; }

        public string GetData()
        {
            return $"{Id} | {Title} | {Price} | {Volume} | {IssueNo}";
        }
    }
}
