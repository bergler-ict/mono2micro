using System;
using System.ComponentModel.DataAnnotations;

namespace SalesService.Data.Models
{
    public class ShoppingBasketItem
    {
        public Guid ShoppingBasketID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public DateTime? DateCreated { get; set;}
        public DateTime? ModifiedDate { get; set; }
    }
}
