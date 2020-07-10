using System.Collections.Immutable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SalesService.Data.Models;
using SalesService.Data.Context;
using SalesService.Web.Models;
using Microsoft.AspNetCore.Http;

namespace SalesService.Web.Controllers.v1
{
    [ApiVersion("1.0")]  
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ShoppingBasketController : ControllerBase
    {
        private readonly ILogger<ShoppingBasketController> _logger;
        private SalesDbContext _salesDbContext;

        public ShoppingBasketController(SalesDbContext salesDbContext,ILogger<ShoppingBasketController> logger)
        {
            _salesDbContext = salesDbContext;
            _logger = logger;
        }

        [HttpGet("{id}",Name = "GetShoppingBasketSummary")]
        [ProducesResponseType(typeof(ShoppingBasketPoco), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModelPoco), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            _logger.LogInformation($"Incoming GET shoppingbasket summary {id}");
            var shoppingBasketItems = await _salesDbContext.ShoppingBasketItems.Where(x => x.ShoppingBasketID == id).ToListAsync<ShoppingBasketItem>();
            //Validate is shoppingbasket is valid
            if (shoppingBasketItems.Count == 0)
            {
                _logger.LogInformation($"Shoppingbasket {id} not found, returning an empty one.");
                return Ok(ShoppingBasketPoco.Empty(id));
            }
            var totalPrice = new decimal();
            var totalQuantity = 0;
            foreach(var item in shoppingBasketItems )
            {
                var shoppingBasketProduct = await _salesDbContext.ShoppingBasketProducts.Where(x => x.ProductId == item.ProductID).FirstOrDefaultAsync();
                //Validate is productid is valid
                if (shoppingBasketProduct == null)
                {
                    return NotFound(new ErrorModelPoco() { ErrorMessage = $"Product with id {item.ProductID} not found" });
                }
                var totalProductPrice = shoppingBasketProduct.Price * item.Quantity;
                totalPrice += totalProductPrice;
                totalQuantity += item.Quantity;
            }
            return Ok(new ShoppingBasketPoco() { ShoppingBasketID = id, TotalPrice = totalPrice, TotalQuantity = totalQuantity});
        }

        [HttpGet("{id}/items",Name = "GetShoppingBasket")]
        [ProducesResponseType(typeof(IEnumerable<ShoppingBasketItemPoco>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModelPoco), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetShoppingCartAsync(Guid id)
        {
            _logger.LogInformation($"Incoming GET shoppingbasket {id}");
            var shoppingBasketItems = await _salesDbContext.ShoppingBasketItems.Where(x => x.ShoppingBasketID == id).ToListAsync<ShoppingBasketItem>();
            //Validate is shoppingbasket is valid
            if (shoppingBasketItems.Count == 0)
            {
                _logger.LogInformation($"Shoppingbasket {id} not found, returning an empty list of items.");
                return Ok(Enumerable.Empty<ShoppingBasketItemPoco>());
            }
            var resultShoppingBasket = new List<ShoppingBasketItemPoco>();

            foreach(var item in shoppingBasketItems )
            {
                var shoppingBasketProduct = await _salesDbContext.ShoppingBasketProducts.Where(x => x.ProductId == item.ProductID).FirstOrDefaultAsync();
                //Validate is productid is valid
                if (shoppingBasketProduct == null)
                {
                    return NotFound(new ErrorModelPoco() { ErrorMessage = $"Product with id {item.ProductID} not found" });
                }
                resultShoppingBasket.Add(new ShoppingBasketItemPoco()
                { 
                    ShoppingBasketID = item.ShoppingBasketID,
                    Quantity = item.Quantity,
                    TotalPrice = shoppingBasketProduct.Price * item.Quantity, 
                    ProductId =  item.ProductID,
                    ProductName = shoppingBasketProduct.ProductName
                });
            }
            return Ok(resultShoppingBasket);
        }

        [HttpPut("{id}/items/{productid}/{quantity}",Name = "AddProductToShoppingCart")]
        [ProducesResponseType(typeof(IEnumerable<ShoppingBasketItemPoco>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModelPoco), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAsync(Guid id, int productid, int quantity) 
        {
            _logger.LogInformation($"Incoming PUT item in shoppingbasket {id} productId:{productid} quantity:{quantity}");
            //Validate is productid is valid
            var shoppingBasketProduct = await _salesDbContext.ShoppingBasketProducts.Where(x => x.ProductId == productid).FirstOrDefaultAsync();
            if (shoppingBasketProduct == null) { 
                return NotFound(new ErrorModelPoco() { ErrorMessage = $"Product with id {productid} not found" });
            }

            var item = _salesDbContext.ShoppingBasketItems.Where(x => x.ProductID == productid && x.ShoppingBasketID == id).SingleOrDefault();
            //If new item and quantity is more then 0
            if (item == null && quantity > 0)
            {
                _salesDbContext.ShoppingBasketItems.Add(new ShoppingBasketItem() { 
                    ShoppingBasketID = id, 
                    ProductID = productid, 
                    Quantity = quantity,
                    DateCreated = DateTime.Now,
                    ModifiedDate = DateTime.Now
                });
            }
            //If existing item and quantity is more then 0
            else if (item != null && quantity > 0)
            {
                item.Quantity = quantity;
                item.ModifiedDate = DateTime.UtcNow;
                _salesDbContext.ShoppingBasketItems.Update(item);
            }
            //If existing item and quantity is 0
            else if (item != null && quantity == 0)
            {
                _salesDbContext.ShoppingBasketItems.Remove(item);
            }
            var result = await _salesDbContext.SaveChangesAsync();
            var actionResult = await GetShoppingCartAsync(id) as OkObjectResult;
            return Ok(actionResult.Value);
        }
    }
}
