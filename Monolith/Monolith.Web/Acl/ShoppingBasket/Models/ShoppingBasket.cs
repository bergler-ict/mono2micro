using Monolith.Web.Models;
using System;
using System.Linq;

namespace Monolith.Web.Acl.ShoppingBasket.Models
{
    public class ShoppingBasket
    {
        public Guid Id { get; private set; }
        public int TotalQuantity { get; private set; }
        public decimal TotalPrice { get; private set; }

        public static ShoppingBasket MapFrom(CartModel cart, Guid cartId)
        {
            return new ShoppingBasket()
            {
                Id = cartId,
                TotalQuantity = cart.Items?.Sum(i => i.Quantity) ?? 0,
                TotalPrice = cart.Items?.Sum(i => i.TotalPrice) ?? 0M
            };
        }
    }
}