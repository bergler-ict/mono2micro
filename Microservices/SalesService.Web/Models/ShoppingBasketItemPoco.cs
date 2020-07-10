using System;

namespace SalesService.Web.Models
{
    public class ShoppingBasketItemPoco
    {
        public Guid ShoppingBasketID { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal TotalPrice { get; set; }
    }
}