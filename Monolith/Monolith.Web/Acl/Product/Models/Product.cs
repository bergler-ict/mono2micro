using Monolith.Web.Controllers;
using Monolith.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monolith.Web.Acl.Product.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public static Product MapFrom(ProductModel product)
        {
            return new Product()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
        }
    }
}