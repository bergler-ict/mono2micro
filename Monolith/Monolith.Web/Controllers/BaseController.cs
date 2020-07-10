using Monolith.Data;
using Monolith.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Monolith.Web.Controllers
{
    public abstract class BaseController<TModel> : Controller
        where TModel : BaseModel, new()
    {
        public TModel Model { get { return CreateModel(); } }

        public BaseController()
        {
        }

        protected virtual TModel CreateModel(Action<TModel> modelSetup = null)
        {
            var model = new TModel()
            {
                ProductCategories = DatabaseFunctions.GetAllProductCategories()
            };
            modelSetup?.Invoke(model);

            return model;
        }
    }
}