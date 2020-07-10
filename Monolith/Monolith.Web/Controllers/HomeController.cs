using Monolith.Data;
using Monolith.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Monolith.Web.Controllers
{
    public class HomeController : BaseController<HomeModel>
    {
        public ActionResult Index()
        {
            return View(Model);
        }

    }
}