using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaFrontend.Blazor.Clients.Models
{
    public class ShoppingBasket
    {
        public Guid Id { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
