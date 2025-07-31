﻿using Core.Entites.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(string email) : base(x => x.BuyerEmail == email) 
        {
            AddInclude(x => x.OrderdItems);
            AddInclude(x => x.DeliveryMethod);
            AddOrderByDesc(x => x.OrderDate);
        }
        public OrderSpecification(string email, int id) : base(x => x.BuyerEmail == email && x.Id == id) 
        {
            AddInclude("OrderdItems");
            AddInclude("DeliveryMethod");
        }
    }
}
