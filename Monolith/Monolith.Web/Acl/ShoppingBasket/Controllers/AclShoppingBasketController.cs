using Monolith.Web.Controllers;
using Monolith.Web.Models;
using System;
using System.Web;
using System.Web.Http;

namespace Monolith.Web.Acl.ShoppingBasket.Controllers
{
    [RoutePrefix("acl/shoppingbasket")]
    public class AclShoppingBasketController : ApiController
    {
        private readonly CartController _legacyController;

        public AclShoppingBasketController(CartController legacyController)
        {
            this._legacyController = legacyController ?? throw new ArgumentNullException(nameof(legacyController));
        }

        [System.Web.Http.Route("{shoppingBasketId}")]
        public Models.ShoppingBasket Get(Guid shoppingBasketId)
        {
            HttpContext.Current.Request.Cookies.Add(new HttpCookie("ShoppingCartId", shoppingBasketId.ToString("D")));
            var viewResult = _legacyController.Index() as System.Web.Mvc.ViewResult;
            var legacyModel = viewResult.Model as CartModel;
            var response = Models.ShoppingBasket.MapFrom(legacyModel, shoppingBasketId);
            HttpContext.Current.Response.Cookies.Clear();

            return response;
        }
    }
}
