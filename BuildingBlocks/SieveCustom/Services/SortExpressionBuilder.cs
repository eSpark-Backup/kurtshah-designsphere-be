using SieveCustom.Models;

namespace SieveCustom.Services;

public static class SortExpressionBuilder<T>
{
    private static IDictionary<SortDirection, ISortExpression> directions = new Dictionary<SortDirection, ISortExpression>
{
    { SortDirection.Ascending, new OrderByAscendingSortExpression() },
    { SortDirection.Descending, new OrderByDescendingSortExpression() }
};

    private interface ISortExpression
    {
        Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> GetExpression();
    }

    private class OrderByAscendingSortExpression : ISortExpression
    {
        public Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> GetExpression()
        {
            return (c, f) => c.OrderBy(f);
        }
    }

    private class OrderByDescendingSortExpression : ISortExpression
    {
        public Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> GetExpression()
        {
            return (c, f) => c.OrderByDescending(f);
        }
    }

    public static Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> CreateExpression(SortDirection direction)
    {
        return directions[direction].GetExpression();
    }
}