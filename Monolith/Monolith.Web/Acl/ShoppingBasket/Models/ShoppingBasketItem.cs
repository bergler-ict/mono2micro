using Monolith.Web.Models;

namespace Monolith.Web.Acl.ShoppingBasket.Models
{
    public class ShoppingBasketItem
    {
        public int ProductID { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal TotalPrice { get; private set; }

        public static ShoppingBasketItem MapFrom(CartItemModel cartItem)
        {
            return new ShoppingBasketItem()
            {
                ProductID = cartItem.Product.Id,
                ProductName = cartItem.Product.Name,
                TotalPrice = cartItem.TotalPrice,
                Quantity = cartItem.Quantity
            };
        }
    }
}