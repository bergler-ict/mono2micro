using Monolith.Data;
using Monolith.Logic;
using Monolith.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Monolith.Web.Controllers
{
    public class CartController : BaseController<CartModel>
    {
        private Cart _cart;

        public CartController()
        {
        }

        protected override CartModel CreateModel(Action<CartModel> modelSetup = null)
        {
            _cart = DatabaseFunctions.GetCartById(Cart.GetSessionCartId());
            var model = CartModel.MapFrom(_cart);
            model.ProductCategories = DatabaseFunctions.GetAllProductCategories();
            return model;
        }
        
        public PartialViewResult HeaderCart()
        {
            return PartialView("CartView", Model);
        }

        public ActionResult Index()
        {
            return View("Index", Model);
        }

        public ActionResult Add(int productIdToAdd)
        {
            var cartId = Cart.GetSessionCartId();
            DatabaseFunctions.AddProductToCart(cartId, productIdToAdd);

            return Index();
        }

        public ActionResult Remove(int productIdToRemove, bool all = false)
        {
            var cartId = Cart.GetSessionCartId();
            DatabaseFunctions.RemoveProductFromCart(cartId, productIdToRemove, all);

            return Index();
        }
    }
}