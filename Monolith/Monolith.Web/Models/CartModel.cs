using Monolith.Logic;
using System.Collections.Generic;
using System.Linq;

namespace Monolith.Web.Models
{
    public class CartModel : BaseModel
    {
        public IEnumerable<CartItemModel> Items { get; set; } = Enumerable.Empty<CartItemModel>();
        public decimal TotalPrice => Items?.Sum(i => i.TotalPrice) ?? 0M;
        public int ItemCount => Items?.Sum(i => i.Quantity) ?? 0;
        public static CartModel MapFrom(Cart cart)
        {
            return new CartModel()
            {
                Items = cart.Items.Select(c => new CartItemModel()
                {
                    Product = ProductModel.MapFrom(c.Product),
                    Quantity = c.Quantity,
                    TotalPrice = c.TotalPrice
                })
            };
        }
    }
}