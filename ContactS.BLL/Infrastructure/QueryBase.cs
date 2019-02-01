using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace ContactS.BLL.Infrastructure
{
    public abstract class QueryBase<TResult> : IQuery<TResult>
    {
        protected QueryBase()
        {
            SortCriteria = new List<Func<IQueryable<TResult>, IOrderedQueryable<TResult>>>();
        }

        public int? Skip { get; set; }

        public int? Take { get; set; }

        public IList<Func<IQueryable<TResult>, IOrderedQueryable<TResult>>> SortCriteria { get; }

        public void AddSortCriteria(string fieldName, SortDirection direction = SortDirection.Ascending)
        {

            PropertyInfo prop = typeof(TResult).GetProperty(fieldName);
            ParameterExpression param = Expression.Parameter(typeof(TResult), "i");
            LambdaExpression expr = Expression.Lambda(Expression.Property(param, prop), param);

            typeof(QueryBase<TResult>).GetMethod(nameof(AddSortCriteriaCore), BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(prop.PropertyType)
                .Invoke(this, new object[] { expr, direction });
        }

        public void AddSortCriteria<TKey>(Expression<Func<TResult, TKey>> field, SortDirection direction)
        {
            AddSortCriteriaCore(field, direction);
        }

        public  IList<TResult> Execute()
        {
            IQueryable<TResult> query = GetQueryable();

            for (int i = SortCriteria.Count - 1; i >= 0; i--)
                query = SortCriteria[i](query);

            if (Skip != null)
                query = query.Skip(Skip.Value);
            if (Take != null)
                query = query.Take(Take.Value);

            List<TResult> results = query.ToList();
            PostProcessResults(results);
            return results;
        }

        public int GetTotalRowCount()
        {
            return GetQueryable().Count();
        }

        private void AddSortCriteriaCore<TKey>(Expression<Func<TResult, TKey>> sortExpression, SortDirection direction)
        {
            if (direction == SortDirection.Ascending)
                SortCriteria.Add(x => x.OrderBy(sortExpression));
            else
                SortCriteria.Add(x => x.OrderByDescending(sortExpression));
        }

        public void ClearSortCriterias()
        {
            SortCriteria.Clear();
        }

        protected virtual void PostProcessResults(IList<TResult> results)
        {
        }

        protected abstract IQueryable<TResult> GetQueryable();
    }
}
