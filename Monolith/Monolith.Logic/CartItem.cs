using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monolith.Logic
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Product.Price * Quantity;
    }
}