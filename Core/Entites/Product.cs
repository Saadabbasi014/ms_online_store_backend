﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites
{
    public class Product : BaseEntity
    {
        //public int Id { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }
        public required string Brand { get; set; }
        public string ImgUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
      
    }
}
