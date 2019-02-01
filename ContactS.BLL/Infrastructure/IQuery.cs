using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ContactS.BLL.Infrastructure
{
    public interface IQuery<TResult>
    {
        int? Skip { get; set; }

        int? Take { get; set; }

        IList<Func<IQueryable<TResult>, IOrderedQueryable<TResult>>> SortCriteria { get; }

        void AddSortCriteria<TKey>(Expression<Func<TResult, TKey>> field, SortDirection direction = SortDirection.Ascending);

        void AddSortCriteria(string fieldName, SortDirection direction = SortDirection.Ascending);

        IList<TResult> Execute();

        int GetTotalRowCount();
    }
}
