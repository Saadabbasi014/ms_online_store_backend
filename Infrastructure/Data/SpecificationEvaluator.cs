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
            return query;
        }

        public static IQueryable<TResult> GetQuery<ISpec, TResult>(IQueryable<T> query,
            ISpecification<T, TResult> spec)
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

            if (spec.Select != null)
            {
                IQueryable<TResult> selectQuery = query.Select(spec.Select);

                if (spec.IsDistinct)
                {
                    selectQuery = selectQuery.Distinct();
                }

                return selectQuery;
            }
            return query.Cast<TResult>();
        }
    }
}
 