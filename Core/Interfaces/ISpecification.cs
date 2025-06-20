using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>>? Criteria { get; }
        Expression<Func<T, object>>? OrderBy { get; }
        Expression<Func<T, object>>? OrderByDesc { get; } 
        bool IsDistinct { get; }
        bool IsPagingEnable { get; }

        int Take { get; }

        int Skip { get; }

        IQueryable<T> ApplyCriteria(IQueryable<T> query);
        //{
        //    if (Criteria != null)
        //    {
        //        query = query.Where(Criteria);
        //    }
        //    if (OrderBy != null)
        //    {
        //        query = query.OrderBy(OrderBy);
        //    }
        //    if (OrderByDesc != null)
        //    {
        //        query = query.OrderByDescending(OrderByDesc);
        //    }
        //    if (IsDistinct)
        //    {
        //        query = query.Distinct();
        //    }
        //    if (IsPagingEnable)
        //    {
        //        query = query.Skip(Skip).Take(Take);
        //    }
        //    return query;
        //}

    }

    public interface ISpecification<T, TResult> : ISpecification<T> 
    {
        Expression<Func<T, TResult>>? Select { get; }
    }

}
