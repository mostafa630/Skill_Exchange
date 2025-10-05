using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; protected set; } = (T) => true;

        public List<Expression<Func<T, object>>> Includes { get; } = new();

        public Expression<Func<T, object>> OrderBy { get; protected set; }

        public Expression<Func<T, object>> OrderByDescending { get; protected set; }

        public int? Take { get; protected set; }

        public int? Skip { get; protected set; }

        public bool IsPagingEnabled { get; protected set; }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

        public BaseSpecification<T> And(Expression<Func<T, bool>> otherCriteria)
        {
            Criteria = CombineExpressions(Criteria!, otherCriteria!, Expression.AndAlso);
            return this;
        }

        public BaseSpecification<T> Or(Expression<Func<T, bool>> otherCriteria)
        {
            Criteria = CombineExpressions(Criteria!, otherCriteria!, Expression.OrElse);
            return this;
        }


        // helper for combining expressions
        private static Expression<Func<T, bool>> CombineExpressions(
            Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right,
            Func<Expression, Expression, BinaryExpression> merge)
        {
            var param = Expression.Parameter(typeof(T));
            var leftBody = Expression.Invoke(left, param);
            var rightBody = Expression.Invoke(right, param);
            return Expression.Lambda<Func<T, bool>>(merge(leftBody, rightBody), param);
        }
    }
}