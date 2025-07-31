using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.OrderAggregate
{
    public class ProductItemOrderd
    {
        public int ProductId { get; set; }
        public required string ProductName { get; set; }
        public required string ImgUrl { get; set; }
    }
}
