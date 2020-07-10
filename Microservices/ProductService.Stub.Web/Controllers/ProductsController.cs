using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using ProductService.Stub.Web.Models;
using Microsoft.AspNetCore.Http;
using ProductService.Stub.MessageContracts;
using MassTransit;
using System.Threading;

namespace ProductService.Stub.Web.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]  
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductsController> _logger;

        private readonly IBus _bus;

        public ProductsController(IBus bus, ILogger<ProductsController> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _bus = bus;
        }

        [HttpPut("{productid}/items/{price}",Name = "AddNewProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PutAsync(int productid,decimal price, CancellationToken cancellationToken)
        {
            var product = new ProductChangedModel() 
            {  
                ProductId = productid,
                Price = price,
                Description = "Really cool product"
            };
            try
            {
                await _bus.Publish(product,cancellationToken);
            }
            catch(Exception ex)
            {
                var msg = ex.Message;
            }
            return Ok();
        }


    }
}
