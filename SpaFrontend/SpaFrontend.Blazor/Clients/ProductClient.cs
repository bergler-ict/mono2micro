using SpaFrontend.Blazor.Clients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SpaFrontend.Blazor.Clients
{
    public class ProductClient : IProductClient
    {
        private readonly HttpClient _httpClient;

        public ProductClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Product>>($"products/category/{category}");
        }

        public async Task<IEnumerable<string>> GetProductCategoriesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<string>>($"productcategory");
        }

        public async Task<string> GetProductImageDataUrlAsync(int productId)
        {
            var fileResult = await _httpClient.GetFromJsonAsync<FileResult>($"products/{productId}/image");
            return $"data:{fileResult.ContentType};base64,{fileResult.FileContents}";
        }
    }
}
