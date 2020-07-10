using Blazored.LocalStorage;
using SpaFrontend.Blazor.Clients;
using SpaFrontend.Blazor.Clients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpaFrontend.Blazor.Services
{
    public class ShoppingBasketService : IShoppingBasketService
    {
        private readonly IShoppingBasketClient _shoppingBasketClient;
        private readonly ILocalStorageService _localStorageService;

        private const string ShoppingBasketIdKey = "ShoppingBasketId";

        public event Action<ShoppingBasket> OnShoppingBasketUpdated;

        public ShoppingBasketService(IShoppingBasketClient shoppingBasketClient, ILocalStorageService localStorageService)
        {
            _shoppingBasketClient = shoppingBasketClient ?? throw new ArgumentNullException(nameof(shoppingBasketClient));
            _localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
        }

        public async Task<IEnumerable<ShoppingBasketItem>> GetShoppingBasketItemsAsync(Guid shoppingBasketId)
        {
            return await _shoppingBasketClient.GetItemsByShoppingBasketId(shoppingBasketId);
        }

        public async Task<ShoppingBasket> GetShoppingBasketAsync()
        {
            var shoppingBaskeyId = await GetShoppingBasketIdAsync();
            return await _shoppingBasketClient.GetByShoppingBasketIdAsync(shoppingBaskeyId);
        }

        public async Task<Guid> GetShoppingBasketIdAsync()
        {
            Guid shoppingBasketId;
            if (await _localStorageService.ContainKeyAsync(ShoppingBasketIdKey))
            {
                shoppingBasketId = await _localStorageService.GetItemAsync<Guid>(ShoppingBasketIdKey);
            }
            else
            {
                shoppingBasketId = Guid.NewGuid();
                await _localStorageService.SetItemAsync(ShoppingBasketIdKey, shoppingBasketId);
            }

            return shoppingBasketId;
        }

        public async Task AddProductAsync(int productId)
        {
            var shoppingBasketId = await GetShoppingBasketIdAsync();
            var shoppingBasketItems = await GetShoppingBasketItemsAsync(shoppingBasketId);
            var currentQuantity = shoppingBasketItems?.FirstOrDefault(item => item.ProductID == productId)?.Quantity ?? 0;
            await _shoppingBasketClient.UpdateShoppingBasket(shoppingBasketId, productId, ++currentQuantity);
            
            await onShoppingBasketUpdated();
        }

        public async Task RemoveProductAsync(int productId, bool all)
        {
            var shoppingBasketId = await GetShoppingBasketIdAsync();
            int currentQuantity = 0;
            if (!all)
            {
                var shoppingBasketItems = await GetShoppingBasketItemsAsync(shoppingBasketId);
                currentQuantity = shoppingBasketItems?.FirstOrDefault(item => item.ProductID == productId)?.Quantity ?? 0;
            }
            await _shoppingBasketClient.UpdateShoppingBasket(shoppingBasketId, productId, all ? 0 : Math.Max(0, --currentQuantity));

            await onShoppingBasketUpdated();
        }

        private async Task onShoppingBasketUpdated()
        {
            OnShoppingBasketUpdated?.Invoke(await GetShoppingBasketAsync());
        }
    }
}
