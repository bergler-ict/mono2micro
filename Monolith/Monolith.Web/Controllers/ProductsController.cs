using AutoMapper;
using Monolith.Data;
using Monolith.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Monolith.Web.Controllers
{
    [RoutePrefix("Products")]
    public class ProductsController : BaseController<ProductsModel>
    {
        // GET: Products
        public ActionResult Index()
        {
            var model = CreateModel();
            return Index(model.ProductCategories.First());
        }

        [Route("{category}")]
        public ActionResult Index(string category)
        {
            return View(CreateModel(m =>
            {
                m.SelectedCategory = category;
                m.ProductModels = ProductModel.MapFrom(DatabaseFunctions.GetProductsByCategory(category));
            }));
        }
    }
}