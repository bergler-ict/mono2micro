using SpaFrontend.Blazor.Clients.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpaFrontend.Blazor.Services
{
    public interface IShoppingBasketService
    {
        event Action<ShoppingBasket> OnShoppingBasketUpdated;
        Task<IEnumerable<ShoppingBasketItem>> GetShoppingBasketItemsAsync(Guid shoppingBasketId);
        Task<ShoppingBasket> GetShoppingBasketAsync();
        Task<Guid> GetShoppingBasketIdAsync();
        Task AddProductAsync(int productId);
        Task RemoveProductAsync(int productId, bool all);
    }
}
