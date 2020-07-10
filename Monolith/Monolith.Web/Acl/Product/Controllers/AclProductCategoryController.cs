using Monolith.Web.Controllers;
using Monolith.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Routing;

namespace Monolith.Web.Acl.Product.Controllers
{
    [RoutePrefix("acl/productcategory")]
    public class AclProductCategoryController : ApiController
    {
        private readonly ProductsController _legacyController;

        public AclProductCategoryController(ProductsController legacyController)
        {
            this._legacyController = legacyController ?? throw new ArgumentNullException(nameof(legacyController));
        }

        [HttpGet]
        [Route()]
        public IEnumerable<string> Get()
        {
            var viewResult = _legacyController.Index() as System.Web.Mvc.ViewResult;
            var legacyModel = viewResult.Model as ProductsModel;
            return legacyModel.ProductCategories;
        }
    }
}
