using System.Linq;
using System;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using SalesService.Web;
using FluentAssertions;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SalesService.Web.Models;

namespace SalesService.Web.Test
{
    public class ShoppingBasketControllerTests: IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ShoppingBasketControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        
        [Fact]
        public async Task GetNonExistingShoppingBasketSummary()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.GetAsync($"/api/v1.0/ShoppingBasket/{Guid.NewGuid()}");
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PutShoppingBasketAddExistingProduct()
        {
            //Arrange
            var client = _factory.CreateClient();
            var shoppingBasketId = Guid.NewGuid();
            var productid = 520;
            var quantity = 2;
            //Act
            var putResponse = await client.PutAsync($"/api/v1.0/ShoppingBasket/{shoppingBasketId}/items/{productid}/{quantity}",null);
            putResponse.EnsureSuccessStatusCode();
            var shoppingBasket = putResponse.ConvertToShoppingBasketItems();
            //Assert
            shoppingBasket.Count.Should().Be(1);
            shoppingBasket[0].ProductId.Should().Be(productid);
            shoppingBasket[0].Quantity.Should().Be(quantity);
            shoppingBasket[0].TotalPrice.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task PutShoppingBasketnotExistingProduct()
        {
            //Arrange
            var client = _factory.CreateClient();
            var shoppingBasketId = Guid.NewGuid();
            var productid = 10;
            var quantity = 2;
            //Act
            var putResponse = await client.PutAsync($"/api/v1.0/ShoppingBasket/{shoppingBasketId}/items/{productid}/{quantity}",null);
            //Assert
            putResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetShoppingBasketSummarySingleProduct()
        {
            //Arrange
            var client = _factory.CreateClient();
            var shoppingBasketId = Guid.NewGuid();
            var productid = 515;
            var quantity = 3;
            var putResponse = client.PutAsync($"/api/v1.0/ShoppingBasket/{shoppingBasketId}/items/{productid}/{quantity}",null).Result;
            putResponse.EnsureSuccessStatusCode();
            //Act
            var getResponse = await client.GetAsync($"/api/v1.0/ShoppingBasket/{shoppingBasketId}");
            getResponse.EnsureSuccessStatusCode();
            var shoppingBasket = getResponse.ConvertToShoppingBasket();
            //Assert
            shoppingBasket.ShoppingBasketID.Should().Be(shoppingBasketId);
            shoppingBasket.TotalQuantity.Should().Be(quantity);
            shoppingBasket.TotalPrice.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetShoppingBasketSummaryMultipleProducts()
        {
            //Arrange
            var client = _factory.CreateClient();
            var shoppingBasketId = Guid.NewGuid();
            var productid1 = 515;
            var quantity1 = 3;
            var putResponse1 = client.PutAsync($"/api/v1.0/ShoppingBasket/{shoppingBasketId}/items/{productid1}/{quantity1}",null).Result;
            putResponse1.EnsureSuccessStatusCode();
            var productid2 = 514;
            var quantity2 = 3;
            var putResponse2 = client.PutAsync($"/api/v1.0/ShoppingBasket/{shoppingBasketId}/items/{productid2}/{quantity2}",null).Result;
            putResponse2.EnsureSuccessStatusCode();
            //Act
            var getResponse = await client.GetAsync($"/api/v1.0/ShoppingBasket/{shoppingBasketId}");
            getResponse.EnsureSuccessStatusCode();
            var shoppingBasket = getResponse.ConvertToShoppingBasket();
            //Assert
            shoppingBasket.ShoppingBasketID.Should().Be(shoppingBasketId);
            shoppingBasket.TotalQuantity.Should().Be(quantity1 + quantity2);
            shoppingBasket.TotalPrice.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetShoppingBasketSingleProduct()
        {
            //Arrange
            var client = _factory.CreateClient();
            var shoppingBasketId = Guid.NewGuid();
            var productid = 515;
            var quantity = 3;
            var putResponse = client.PutAsync($"/api/v1.0/ShoppingBasket/{shoppingBasketId}/items/{productid}/{quantity}",null).Result;
            putResponse.EnsureSuccessStatusCode();
            //Act
            var getResponse = await client.GetAsync($"/api/v1.0/ShoppingBasket/{shoppingBasketId}/items");
            getResponse.EnsureSuccessStatusCode();
            var shoppingBasket = getResponse.ConvertToShoppingBasketItems();
            //Assert
            shoppingBasket.Count.Should().Be(1);
            shoppingBasket[0].ProductId.Should().Be(productid);
            shoppingBasket[0].Quantity.Should().Be(quantity);
            shoppingBasket[0].TotalPrice.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetShoppingBasketMultipleProducts()
        {
            //Arrange
            var client = _factory.CreateClient();
            var shoppingBasketId = Guid.NewGuid();
            var productid1 = 515;
            var quantity1 = 3;
            var putResponse1 = client.PutAsync($"/api/v1.0/ShoppingBasket/{shoppingBasketId}/items/{productid1}/{quantity1}",null).Result;
            putResponse1.EnsureSuccessStatusCode();
            var productid2 = 514;
            var quantity2 = 3;
            var putResponse2 = client.PutAsync($"/api/v1.0/ShoppingBasket/{shoppingBasketId}/items/{productid2}/{quantity2}",null).Result;
            putResponse2.EnsureSuccessStatusCode();
            //Act
            var getResponse = await client.GetAsync($"/api/v1.0/ShoppingBasket/{shoppingBasketId}/items");
            getResponse.EnsureSuccessStatusCode();
            var shoppingBasket = getResponse.ConvertToShoppingBasketItems();
            //Assert
            shoppingBasket.Count.Should().Be(2);
            var product1Result = shoppingBasket.Where(x => x.ProductId == productid1).FirstOrDefault();
            product1Result.Quantity.Should().Be(quantity1);
            product1Result.TotalPrice.Should().BeGreaterThan(0);
            var product2Result = shoppingBasket.Where(x => x.ProductId == productid2).FirstOrDefault();
            product2Result.Quantity.Should().Be(quantity2);
            product2Result.TotalPrice.Should().BeGreaterThan(0);
        }
    }
}
