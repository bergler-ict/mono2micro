using System;
using Xunit;
using SalesService.Data.Context;
using FluentAssertions;
using System.Collections.Generic;
using SalesService.Data.Models;
using System.Linq;

namespace SalesService.Data.Test
{
    public class ShoppingBasketItemTests: SalesServiceDataBase
    {
        
        [Fact]
        public void SimpleHappyFlow_ShoppingBasketItemTest()
        {
            //Arrange
            var currentDateTime = DateTime.UtcNow;
            var shoppingCartItems = new List<ShoppingBasketItem>();
            var shoppingBasketId = Guid.NewGuid();
            //seedProducts.Add(new ShoppingBasketProduct() {ProductId =10, ProductName="ML Mountain Seat Assembly",Price = 147.14m }); 
            shoppingCartItems.Add(new ShoppingBasketItem() {ShoppingBasketID = shoppingBasketId, ProductID = 514, Quantity = 4,DateCreated = currentDateTime,ModifiedDate = currentDateTime });
            shoppingCartItems.Add(new ShoppingBasketItem() {ShoppingBasketID = shoppingBasketId, ProductID = 515, Quantity = 2,DateCreated = currentDateTime,ModifiedDate = currentDateTime });
            using (var context = new SalesDbContext(_options))
            {	  
                foreach(var item in shoppingCartItems)              
                {
                    context.ShoppingBasketItems.Add(item);
                }
                //Act
                context.SaveChanges();
                //Assert
                var results = context.ShoppingBasketItems.Where(x => x.ShoppingBasketID == shoppingBasketId);
                results.Should().NotBeNullOrEmpty();
                results.Count().Should().Be(2);
                var prod1 = results.Where(x => x.ProductID == 514).SingleOrDefault();
                var prod2 = results.Where(x => x.ProductID == 515).SingleOrDefault();
                prod1.Should().NotBeNull();
                prod1.Quantity.Should().Be(4);
                prod1.DateCreated.Should().Be(currentDateTime);
                prod1.ModifiedDate.Should().Be(currentDateTime);

                prod2.Should().NotBeNull();
                prod2.Quantity.Should().Be(2);
                prod2.DateCreated.Should().Be(currentDateTime);
                prod2.ModifiedDate.Should().Be(currentDateTime);
            }
        }
    }
}
