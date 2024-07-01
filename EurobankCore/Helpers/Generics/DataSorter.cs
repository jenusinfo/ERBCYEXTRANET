using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Eurobank.Helpers.Generics
{
    public class GenericSorter<T>
    {
        public static IEnumerable<T> Sort(IEnumerable<T> source, string sortBy, SortDirection sortDirection)
        {
            if(source != null && !string.IsNullOrEmpty(sortBy))
			{
                var param = Expression.Parameter(typeof(T), "item");

                var sortExpression = Expression.Lambda<Func<T, object>>
                    (Expression.Convert(Expression.Property(param, sortBy), typeof(object)), param);

                switch(sortDirection.ToString().ToLower())
                {
                    case "asc":
                        return source.AsQueryable<T>().OrderBy<T, object>(sortExpression);
                    default:
                        return source.AsQueryable<T>().OrderByDescending<T, object>(sortExpression);

                }
            }
            return null;
        }
    }

    public enum SortDirection
	{
        asc,
        desc
	}
}
