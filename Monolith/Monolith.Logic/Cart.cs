using System;
using System.Collections.Generic;
using System.Web;

namespace Monolith.Logic
{
    public class Cart
    {
        public string Id { get; set; }
        public List<CartItem> Items { get; set; }

        public Cart(string id, List<CartItem> cartItems)
        {
            Id = id;
            Items = cartItems;
        }

        public static string GetSessionCartId()
        {
            Guid cartId;
            var cookie = HttpContext.Current.Request.Cookies["ShoppingCartId"];
            if (cookie != null)
            {
                if (!Guid.TryParse(cookie.Value, out cartId))
                {
                    cartId = Guid.NewGuid();
                }
            }
            else
            {
                cartId = Guid.NewGuid();
            }

            var id = cartId.ToString("D").ToLower();

            HttpContext.Current.Response.Cookies.Remove("ShoppingCartId");
            HttpContext.Current.Response.Cookies.Add(
                    new HttpCookie("ShoppingCartId", id));

            return id;
        }
    }
}