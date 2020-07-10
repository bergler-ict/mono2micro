using Monolith.Data;
using Monolith.Web.Controllers;
using Monolith.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Monolith.Web.Controllers
{
    [RoutePrefix("Products/{id}")]
    public class ProductImageController : BaseController<HomeModel>
    {
        [Route("Image")]
        public ActionResult Index(int id)
        {
            var imageContent = DatabaseFunctions.GetProductImageContent(id);
            return new FileContentResult(imageContent, @"image/gif");
        }
    }
}