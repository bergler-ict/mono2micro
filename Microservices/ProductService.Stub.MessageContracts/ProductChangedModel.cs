using System;

namespace ProductService.Stub.MessageContracts
{
    public class ProductChangedModel
    {
        public int ProductId { get; set; }

        public decimal Price { get; set; }

        public string Description {get; set; }
    }
}
