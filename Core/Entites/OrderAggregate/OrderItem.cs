using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.OrderAggregate
{
    public class OrderItem : BaseEntity
    {
        public ProductItemOrderd ItemOrderd { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
