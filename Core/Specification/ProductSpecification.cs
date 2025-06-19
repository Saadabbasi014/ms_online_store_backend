using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(string? brand, string? type, string? sort) : base(p =>
            (string.IsNullOrEmpty(brand) || p.Brand == brand) &&
            (string.IsNullOrEmpty(type) || p.Type == type))
        {
            switch (sort)
            {
                case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;

                case "priceDesc":
                    AddOrderByDesc(p => p.Price);
                    break;

                default:
                    AddOrderBy(p => p.Name);
                    break;
            }
        }
    }
}
 