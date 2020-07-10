using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesService.Data.Models
{
    public class ShoppingBasketProduct
    {
        [Key]
        public Guid Id { get; set;}
        public int ProductId {get; set;}
        public string ProductName { get; set; }
        [Column(TypeName="money")]
        public decimal Price { get; set; }
    }
}