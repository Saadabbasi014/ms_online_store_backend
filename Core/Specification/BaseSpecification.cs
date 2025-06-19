using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification
{
    public class BaseSpecification<T>(Expression<Func<T, bool>>? _criteria) : ISpecification<T>
    {
        protected BaseSpecification() : this(null) { }

        //private readonly Expression<Func<T, bool>>? _criteria;

        //public BaseSpecification(Expression<Func<T, bool>>? criteria)
        //{
        //    _criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
        //}

        public Expression<Func<T, bool>>? Criteria => _criteria;

        public Expression<Func<T, object>>? OrderBy{ get ;  private set;}

        public Expression<Func<T, object>>? OrderByDesc { get; private set;}

        public bool IsDistinct { get; private set; }

        protected void AddOrderBy(Expression<Func<T, object>>? orderBy)
        {
            OrderBy = orderBy ?? throw new ArgumentNullException(nameof(orderBy));
        }

        protected void AddOrderByDesc(Expression<Func<T, object>>? orderBy)
        {
            OrderByDesc = orderBy ?? throw new ArgumentNullException(nameof(orderBy));
        }

        protected void ApplyDistinct()
        {
            IsDistinct = true;
        }
    }


    public class BaseSpecification<T, TResult>(Expression<Func<T, bool>> criteria) :
        BaseSpecification<T>(criteria), ISpecification<T, TResult>
    {
        protected BaseSpecification() : this(null!) { }
        public Expression<Func<T, TResult>>? Select { get; private set; }

        protected void AddSelect(Expression<Func<T, TResult>>? select)
        {
            Select = select ?? throw new ArgumentNullException(nameof(select));
        }
    }
}
 