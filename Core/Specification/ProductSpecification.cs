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
        //public ProductSpecification(ProductSpecParams productSpecs) : base(x =>
        //    (string.IsNullOrEmpty(productSpecs.Search) || x.Name.ToLower().Contains(productSpecs.Search)) &&
        //    (productSpecs.Brands.Count > 0) || productSpecs.Brands.Contains(x.Brand) &&
        //    (productSpecs.Types.Count > 0) || productSpecs.Types.Contains(x.Type)
        //) 
            public ProductSpecification(ProductSpecParams productSpecs) : base(x =>
                (string.IsNullOrEmpty(productSpecs.Search) || x.Name.ToLower().Contains(productSpecs.Search)) &&
                (productSpecs.Brands.Count == 0 || productSpecs.Brands.Contains(x.Brand)) &&
                (productSpecs.Types.Count == 0 || productSpecs.Types.Contains(x.Type))
            )

            {
            ApplyPaging(productSpecs.PageSize * (productSpecs.PageIndex - 1), productSpecs.PageSize);

            switch (productSpecs.Sort)
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
 