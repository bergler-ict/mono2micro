using Newtonsoft.Json;
using SpaFrontend.Blazor.Clients.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SpaFrontend.Blazor.Clients
{
    public class ShoppingBasketClient : IShoppingBasketClient
    {
        private readonly HttpClient _httpClient;

        public ShoppingBasketClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<ShoppingBasket> GetByShoppingBasketIdAsync(Guid shoppingBasketId)
        {
            return await _httpClient.GetFromJsonAsync<ShoppingBasket>($"shoppingbasket/{shoppingBasketId:D}");
        }

        public async Task<IEnumerable<ShoppingBasketItem>> GetItemsByShoppingBasketId(Guid shoppingBasketId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ShoppingBasketItem>>($"shoppingbasket/{shoppingBasketId:D}/items");
        }

        public async Task<IEnumerable<ShoppingBasketItem>> UpdateShoppingBasket(Guid shoppingBasketId, int productId, int newQuantity)
        {
            var response = await _httpClient.PutAsJsonAsync($"shoppingbasket/{shoppingBasketId:D}/items/{productId}/{newQuantity}", (string)null);
            var body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ShoppingBasketItem>>(body);
        }
    }
}
