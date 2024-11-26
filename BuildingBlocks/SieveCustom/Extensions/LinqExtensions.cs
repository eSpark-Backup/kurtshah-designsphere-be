using SieveCustom.Models;
using SieveCustom.Services;
using System.Linq.Expressions;

namespace SieveCustom.Extensions;

public static class LinqCustomExtensions
{
    public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> collection, SieveModel model)
    {
        if (model?.GetSortsParsed() == null)
        {
            return collection;
        }
        var sorts = model.GetSortsParsed();

        ParameterExpression param = Expression.Parameter(typeof(T), "x"); // x
        Expression property = Expression.Property(param, sorts[0].Name);     // x.ColumnName
        Func<T, object> func = Expression.Lambda<Func<T, object>>(        // x => x.ColumnName
           Expression.Convert(Expression.Property(param, sorts[0].Name),
           typeof(object)), param
        ).Compile();

        Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> expression =
            SortExpressionBuilder<T>.CreateExpression(sorts[0].Descending ? SortDirection.Descending : SortDirection.Ascending);
        var sorted = expression(collection, func);
        return sorted;
    }
}