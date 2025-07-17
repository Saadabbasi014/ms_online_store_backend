using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public required string ProductName { get; set; }
        public string? ImgUrl { get; set; }
        public required string Type { get; set; }
        public required string Brand { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } 
    }
}
