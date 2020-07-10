using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monolith.Data.Dtos
{
    public class ShoppingCartItem
    {
        public string ShoppingCartID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
