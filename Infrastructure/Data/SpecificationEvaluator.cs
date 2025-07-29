using Core.Entites;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
        {
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria); // x => x.Brand == brand 
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDesc != null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            if (spec.IsDistinct)
            {
                query = query.Distinct();
            }

            if (spec.IsPagingEnable)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            return query;
        }

        //public static IQueryable<TResult> GetQuery<ISpec, TResult>(IQueryable<T> query,
        //    ISpecification<T, TResult> spec)
        //{
        //    if (spec.Criteria != null)
        //    {
        //        query = query.Where(spec.Criteria); // x => x.Brand == brand 
        //    }

        //    if (spec.OrderBy != null)
        //    {
        //        query = query.OrderBy(spec.OrderBy);
        //    }

        //    if (spec.OrderByDesc != null)
        //    {
        //        query = query.OrderByDescending(spec.OrderByDesc);
        //    }

        //    IQueryable<TResult>? selectQuery = query as IQueryable<TResult>;

        //    if (spec.Select != null)
        //    {
        //        selectQuery = query.Select(spec.Select);
        //    }

        //    if (spec.IsDistinct)
        //    {
        //        selectQuery = selectQuery?.Distinct();
        //    }

        //    if (spec.IsPagingEnable)
        //    {
        //        selectQuery = selectQuery?.Skip(spec.Skip).Take(spec.Take);
        //    }

        //    return query.Cast<TResult>();
        //}

        public static IQueryable<TResult> GetQuery<TResult>(IQueryable<T> query, ISpecification<T, TResult> spec)
        {
            // Apply filtering
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            // Apply ordering
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDesc != null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            // Apply projection (SELECT)
            if (spec.Select == null)
            {
                throw new InvalidOperationException("Select expression must be provided for TResult projections.");
            }

            var selectQuery = query.Select(spec.Select);

            // Apply distinct
            if (spec.IsDistinct)
            {
                selectQuery = selectQuery.Distinct();
            }

            // Apply paging
            if (spec.IsPagingEnable)
            {
                selectQuery = selectQuery.Skip(spec.Skip).Take(spec.Take);
            }

            return selectQuery;
        }

    }
}
 