using SpaFrontend.Blazor.Clients;
using SpaFrontend.Blazor.Clients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpaFrontend.Blazor.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductClient _productClient;

        public ProductsService(IProductClient productClient)
        {
            _productClient = productClient ?? throw new ArgumentNullException(nameof(productClient));
        }

        public async Task<IEnumerable<string>> GetProductCategoriesAsync()
        {
            return await _productClient.GetProductCategoriesAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
        {
            return await _productClient.GetByCategoryAsync(category);
        }

        public async Task<string> GetProductImageDataUrlAsync(int productId)
        {
            return await _productClient.GetProductImageDataUrlAsync(productId);
        }
    }
}
