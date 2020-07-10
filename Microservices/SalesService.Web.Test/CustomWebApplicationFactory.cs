using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SalesService.Data;
using SalesService.Data.Context;
using SalesService.Data.Models;

namespace SalesService.Web.Test
{
    #region snippet1
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup: class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's ApplicationDbContext registration.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<SalesDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add ApplicationDbContext using an in-memory database for testing.
                services.AddDbContext<SalesDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    //options.UseSqlServer("Server=localhost;Database=SalesDb;Trusted_Connection=True;MultipleActiveResultSets=true");
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<SalesDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    // Ensure the database is created.
                    db.Database.EnsureCreated();

                    try
                    {
                        var seedProducts = new List<ShoppingBasketProduct>();
                        seedProducts.Add(new ShoppingBasketProduct() { ProductId = 515, ProductName="ML Mountain Seat Assembly",Price = 147.14m });
                        seedProducts.Add(new ShoppingBasketProduct() { ProductId = 516, ProductName="HL Mountain Seat Assembly",Price = 196.92m });
                        seedProducts.Add(new ShoppingBasketProduct() { ProductId = 517, ProductName="LL Road Seat Assembly",Price = 133.34m });
                        seedProducts.Add(new ShoppingBasketProduct() { ProductId = 514, ProductName="LL Mountain Seat Assembly",Price = 133.34m });
                        seedProducts.Add(new ShoppingBasketProduct() { ProductId = 518, ProductName="ML Road Seat Assembly",Price = 147.14m });
                        seedProducts.Add(new ShoppingBasketProduct() { ProductId = 519, ProductName="HL Road Seat Assembly",Price = 196.92m });
                        seedProducts.Add(new ShoppingBasketProduct() { ProductId = 520, ProductName="LL Touring Seat Assembly",Price =	133.34m });
                        seedProducts.Add(new ShoppingBasketProduct() { ProductId = 521, ProductName="ML Touring Seat Assembly",Price =	147.14m });
                        seedProducts.Add(new ShoppingBasketProduct() { ProductId = 522, ProductName="HL Touring Seat Assembly",Price =	196.92m });
                        seedProducts.Add(new ShoppingBasketProduct() { ProductId = 680, ProductName="HL Road Frame - Black, 58",Price = 1431.50m });
                        seedProducts.Add(new ShoppingBasketProduct() { ProductId = 706, ProductName="HL Road Frame - Red, 58",Price = 1431.50m });
                        seedProducts.Add(new ShoppingBasketProduct() { ProductId = 707, ProductName="Sport-100 Helmet, Red",Price = 34.99m });
                        seedProducts.Add(new ShoppingBasketProduct() { ProductId = 708, ProductName="Sport-100 Helmet, Black",Price = 34.99m });
                        seedProducts.Add(new ShoppingBasketProduct() { ProductId = 709, ProductName="Mountain Bike Socks, M",Price = 9.50m });
                        seedProducts.Add(new ShoppingBasketProduct() { ProductId = 710, ProductName="Mountain Bike Socks, L",Price = 9.50m });
                        // Seed the database with test data.
                        foreach(var item in seedProducts)
                        {
                            db.ShoppingBasketProducts.Add(item);
                        }
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
    #endregion
}