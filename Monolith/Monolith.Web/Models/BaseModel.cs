using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monolith.Web.Models
{
    public abstract class BaseModel
    {
        public IEnumerable<string> ProductCategories { get; set; }
    }
}