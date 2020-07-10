using Monolith.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monolith.Web.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Number { get; set; }
        public string ImageUrl => $"/Products/{Id}/Image";
        public decimal Price { get; set; }

        public static ProductModel MapFrom(Product product)
        {
            return new ProductModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Number = product.Number,
                Price = product.Price
            };
        }

        public static IEnumerable<ProductModel> MapFrom(IEnumerable<Product> products)
        {
            return products?.Select(p => MapFrom(p));
        }
    }
}