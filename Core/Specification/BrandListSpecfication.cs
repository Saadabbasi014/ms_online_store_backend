using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification
{
    public class BrandListSpecfication : BaseSpecification<Product, string>
    {
        public BrandListSpecfication()
        {
            AddSelect(p => p.Brand);
            ApplyDistinct();
        }
    }
}
