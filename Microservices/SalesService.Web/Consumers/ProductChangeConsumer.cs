using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using ProductService.Stub.MessageContracts;
using SalesService.Data.Context;
using SalesService.Data.Models;

namespace SalesService.Web.Consumer
{
    public class ProductChangeConsumer : IConsumer<ProductChangedModel>
    {
        ILogger<ProductChangeConsumer> _logger; 
        SalesDbContext _salesDbContext;
        public ProductChangeConsumer(ILogger<ProductChangeConsumer> logger,SalesDbContext salesDbContext)
        {
            _logger = logger;
            _salesDbContext = salesDbContext;
        }
        public Task Consume(ConsumeContext<ProductChangedModel> context)
        {
            _logger.LogInformation("Consume incoming event");
            var shoppingBasketProduct = _salesDbContext.ShoppingBasketProducts.Where(x => x.ProductId == context.Message.ProductId).FirstOrDefault();
            //Validate is productid is valid
            if (shoppingBasketProduct == null)
            {
                _salesDbContext.ShoppingBasketProducts.Add(new ShoppingBasketProduct() { ProductId = context.Message.ProductId, Price = context.Message.Price, ProductName = context.Message.Description} );
            }
            else
            {
                shoppingBasketProduct.Price = context.Message.Price;
            }
            var result = _salesDbContext.SaveChanges();
            _logger.LogInformation("Incoming event consumed");
            return Task.FromResult(result);
        }
    }
}