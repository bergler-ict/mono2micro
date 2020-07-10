using SpaFrontend.Blazor.Clients.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpaFrontend.Blazor.Services
{
    public interface IProductsService
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
        Task<IEnumerable<string>> GetProductCategoriesAsync();
        Task<string> GetProductImageDataUrlAsync(int productId);
    }
}