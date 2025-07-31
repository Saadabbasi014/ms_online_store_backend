using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.OrderAggregate
{
    public enum OrderStatus
    {   
        Pending,
        PaymentRecieved,
        PaymentFailed
    }
}
