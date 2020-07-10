using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monolith.Web.Models
{
    public class ProductsModel : BaseModel
    {
        public string SelectedCategory { get; set; }
        public IEnumerable<ProductModel> ProductModels { get; set; }
    }
}