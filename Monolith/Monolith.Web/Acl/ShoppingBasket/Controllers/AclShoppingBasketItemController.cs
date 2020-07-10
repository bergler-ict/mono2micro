using Monolith.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Monolith.Web.Acl.ShoppingBasket.Controllers
{
    [RoutePrefix("acl/shoppingbasket")]
    public class AclShoppingBasketItemController : ApiController
    {
        private readonly CartController _legacyController;

        public AclShoppingBasketItemController(CartController legacyController)
        {
            this._legacyController = legacyController ?? throw new ArgumentNullException(nameof(legacyController));
        }

        [HttpGet]
        [Route("{shoppingBasketId}/items")]
        public IEnumerable<Models.ShoppingBasketItem> Get(Guid shoppingBasketId) 
        {
            HttpContext.Current.Request.Cookies.Add(new HttpCookie("ShoppingCartId", shoppingBasketId.ToString("D")));
            var response = _legacyController.Model.Items?.Select(i => Models.ShoppingBasketItem.MapFrom(i)) ?? Enumerable.Empty<Models.ShoppingBasketItem>();
            HttpContext.Current.Response.Cookies.Clear();
            
            return response;
        }

        [HttpPut]
        [Route("{shoppingBasketId}/items/{productId}/{newQuantity}")]
        public IEnumerable<Models.ShoppingBasketItem> Put(Guid shoppingBasketId, int productId, int newQuantity)
        {
            HttpContext.Current.Request.Cookies.Add(new HttpCookie("ShoppingCartId", shoppingBasketId.ToString("D")));
            var currentQuantity = _legacyController.Model.Items.FirstOrDefault(i => i.Product.Id == productId)?.Quantity ?? 0;
            if (currentQuantity != newQuantity)
            {
                if (newQuantity < currentQuantity)
                {
                    for (int iteration = 0; iteration < (currentQuantity - newQuantity); iteration++)
                    {
                        _legacyController.Remove(productId);
                    }
                }
                else
                {
                    for (int iteration = 0; iteration < (newQuantity - currentQuantity); iteration++)
                    {
                        _legacyController.Add(productId);
                    }
                }
            }

            return Get(shoppingBasketId);
        }
    }
}
