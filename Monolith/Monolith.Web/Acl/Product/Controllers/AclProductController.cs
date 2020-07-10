using Monolith.Web.Controllers;
using Monolith.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;
using WebApi.OutputCache.V2;

namespace Monolith.Web.Acl.Product.Controllers
{
    [RoutePrefix("acl/products")]
    public class AclProductController : ApiController
    {
        private readonly ProductsController _legacyProductController;
        private readonly ProductImageController _legacyProductImageController;

        public AclProductController(ProductsController legacyProductController, ProductImageController legacyProductImageController)
        {
            _legacyProductController = legacyProductController ?? throw new ArgumentNullException(nameof(legacyProductController));
            _legacyProductImageController = legacyProductImageController ?? throw new ArgumentNullException(nameof(legacyProductImageController));
        }

        [HttpGet]
        [Route("category/{category}")]
        public IEnumerable<Models.Product> Get(string category)
        {
            var viewResult = _legacyProductController.Index(category) as System.Web.Mvc.ViewResult;
            var response = viewResult.Model as ProductsModel;
            return response.ProductModels.Select(p => Models.Product.MapFrom(p));
        }

        [HttpGet]
        [Route("{productId}/image")]
        [CacheOutput(ClientTimeSpan = 600, ServerTimeSpan = 600)]
        public System.Web.Mvc.FileResult Get(int productId)
        {
            return _legacyProductImageController.Index(productId) as System.Web.Mvc.FileContentResult;
        }
    }
}