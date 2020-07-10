using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using SalesService.Web.Models;

namespace SalesService.Web.Test
{
    public static class Extensions
    {
        public static ShoppingBasketPoco ConvertToShoppingBasket (this HttpResponseMessage response)
        {
            var content = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ShoppingBasketPoco>(content);
        }

        public static List<ShoppingBasketItemPoco> ConvertToShoppingBasketItems (this HttpResponseMessage response)
        {
            var content = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<ShoppingBasketItemPoco>>(content);
        }
    }
}