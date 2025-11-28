using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Restaurant : IItemValidating
    {
        public string GetCardPartial()
        {
            return "resturantPartial.cshtml";
        }

        public string GetValiators()
        {
            return "ryan.attard@mcast.edu.mt";
        }
    }
}
