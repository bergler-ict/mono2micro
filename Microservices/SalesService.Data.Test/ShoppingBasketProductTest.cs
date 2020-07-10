using System;
using Xunit;
using System.Linq;
using SalesService.Data.Context;
using System.Collections.Generic;
using SalesService.Data.Models;
using FluentAssertions;

namespace SalesService.Data.Test
{
    public class ShoppingBasketProductTests: SalesServiceDataBase
    {
        [Fact]
        public void  SeedShoppingBasketProductsTest()
        {
            //Arrange
            var seedProducts = new List<ShoppingBasketProduct>();
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 952 , ProductName="Chain",Price = 20.24m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 739 , ProductName="HL Mountain Frame",Price = 1364.50m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 937 , ProductName="HL Mountain Pedal",Price = 80.99m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 930 , ProductName="HL Mountain Tire",Price = 35.00m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 940 , ProductName="HL Road Pedal",Price = 80.99m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 933 , ProductName="HL Road Tire",Price = 32.60m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 808 , ProductName="LL Mountain Handlebars",Price = 44.54m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 935 , ProductName="LL Mountain Pedal",Price = 40.49m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 908 , ProductName="LL Mountain Seat/Saddle 2",Price = 27.12m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 938 , ProductName="LL Road Pedal",Price = 40.49m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 886 , ProductName="LL Touring Frame",Price = 333.42m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 713 , ProductName="Long-Sleeve Logo Jersey",Price = 49.99m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 936 , ProductName="ML Mountain Pedal",Price = 62.09m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 939 , ProductName="ML Road Pedal",Price = 62.09m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 779 , ProductName="Mountain-200",Price = 2319.99m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 980 , ProductName="Mountain-400-W",Price = 769.49m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 873 , ProductName="Patch kit",Price = 2.29m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 894 , ProductName="Rear Derailleur",Price = 121.46m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 872 , ProductName="Road Bottle Cage",Price = 8.99m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 973 , ProductName="Road-350-W",Price = 1700.99m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 797 , ProductName="Road-550-W",Price = 1120.49m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 977 , ProductName="Road-750",Price = 539.99m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 881 , ProductName="Short-Sleeve Classic Jersey",Price = 53.99m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 941 , ProductName="Touring Pedal",Price = 80.99m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 923 , ProductName="Touring Tire Tube",Price = 4.99m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 954 , ProductName="Touring-1000",Price = 2384.07m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 953 , ProductName="Touring-2000",Price = 1214.85m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 958 , ProductName="Touring-3000",Price = 742.35m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 870 , ProductName="Water Bottle",Price = 4.99m });
            seedProducts.Add(new ShoppingBasketProduct() { ProductId = 867 , ProductName="Women's Mountain Shorts",Price = 69.99m });

            using (var context = new SalesDbContext(_options))
            {	                
                var test = context.ShoppingBasketProducts.Where(x => x.ProductId == 1);
                foreach(var item in seedProducts)
                {
                    context.ShoppingBasketProducts.Add(item);
                }
                //Act
                context.SaveChanges();
                //Assert
                var ShoppingBasketProduct =  context.ShoppingBasketProducts.Where(x => x.ProductId == 952).SingleOrDefault();
                ShoppingBasketProduct.Should().NotBeNull();
                ShoppingBasketProduct.ProductId.Should().Be(952);
                ShoppingBasketProduct.ProductName.Should().Be("Chain");
                ShoppingBasketProduct.Price.Should().Be(20.24m);
            }
        }

    }
}