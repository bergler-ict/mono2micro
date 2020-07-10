using SpaFrontend.Blazor.Clients.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpaFrontend.Blazor.Clients
{
    public interface IShoppingBasketClient
    {
        Task<ShoppingBasket> GetByShoppingBasketIdAsync(Guid shoppingBasketId);
        Task<IEnumerable<ShoppingBasketItem>> GetItemsByShoppingBasketId(Guid shoppingBasketId);
        Task<IEnumerable<ShoppingBasketItem>> UpdateShoppingBasket(Guid shoppingBasketId, int productId, int newQuantity);
    }
}