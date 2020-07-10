using System.Collections;
using System;
namespace SalesService.Web.Models
{
    public class ShoppingBasketPoco
    {
        public Guid ShoppingBasketID { get; set; }

        public decimal TotalPrice { get; set; }

        public int TotalQuantity { get; set;}

        public static ShoppingBasketPoco Empty(Guid shoppingBasketID)
        {
            return new ShoppingBasketPoco()
            {
                ShoppingBasketID = shoppingBasketID,
                TotalPrice = 0M,
                TotalQuantity = 0
            };
        }
    }
}