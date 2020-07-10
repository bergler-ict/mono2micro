using SpaFrontend.Blazor.Clients.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpaFrontend.Blazor.Clients
{
    public interface IProductClient
    {
        Task<IEnumerable<Product>> GetByCategoryAsync(string category);
        Task<IEnumerable<string>> GetProductCategoriesAsync();
        Task<string> GetProductImageDataUrlAsync(int productId);
    }
}