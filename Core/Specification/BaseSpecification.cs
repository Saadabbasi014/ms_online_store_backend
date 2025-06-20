using Core.Interfaces;
using System.Linq.Expressions;

namespace Core.Specification
{
    public class BaseSpecification<T>(Expression<Func<T, bool>>? _criteria) : ISpecification<T>
    {
        protected BaseSpecification() : this(null) { }

        public Expression<Func<T, bool>>? Criteria => _criteria;

        public Expression<Func<T, object>>? OrderBy{ get ;  private set;}

        public Expression<Func<T, object>>? OrderByDesc { get; private set;}

        public bool IsDistinct { get; private set; }

        public bool IsPagingEnable { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public IQueryable<T> ApplyCriteria(IQueryable<T> query)
        {
            if (Criteria != null)
            {
                query = query.Where(Criteria);
            }
            return query;
        }

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

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnable = true;
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
 