using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using XelerationTask.Core.Models;

namespace XelerationTask.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyFiltering<T>(this IQueryable<T> query, QueryParameters parameters)
        {
            if (parameters.Filters == null || !parameters.Filters.Any())
                return query;

            var entityType = typeof(T);
            var properties = entityType.GetProperties()
                .Where(p => p.CanRead &&
                           (p.PropertyType == typeof(string) ||
                            p.PropertyType == typeof(int) ||
                            p.PropertyType == typeof(DateTime) ||
                            p.PropertyType == typeof(bool) ||
                            p.PropertyType == typeof(decimal) ||
                            p.PropertyType == typeof(double) ||
                            p.PropertyType.IsEnum))
                .ToDictionary(p => p.Name.ToLower(), p => p);

            foreach (var filter in parameters.Filters)
            {
                string propertyName = filter.Key;
                string filterValue = filter.Value;

                if (string.IsNullOrEmpty(filterValue))
                    continue;

                if (properties.TryGetValue(propertyName.ToLower(), out PropertyInfo? property))
                {
                    var parameter = Expression.Parameter(entityType, "x");

                    var propertyAccess = Expression.Property(parameter, property);

                    Expression comparison;

                    if (property.PropertyType == typeof(string))
                    {
                        MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
                        var constant = Expression.Constant(filterValue);
                        comparison = Expression.Call(propertyAccess, containsMethod, constant);
                    }
                    else if (property.PropertyType == typeof(int) && int.TryParse(filterValue, out int intValue))
                    {
                        var constant = Expression.Constant(intValue);
                        comparison = Expression.Equal(propertyAccess, constant);
                    }
                    else if (property.PropertyType == typeof(DateTime) && DateTime.TryParse(filterValue, out DateTime dateValue))
                    {
                        var constant = Expression.Constant(dateValue);
                        comparison = Expression.Equal(propertyAccess, constant);
                    }
                    else if (property.PropertyType == typeof(bool) && bool.TryParse(filterValue, out bool boolValue))
                    {
                        var constant = Expression.Constant(boolValue);
                        comparison = Expression.Equal(propertyAccess, constant);
                    }
                    else
                    {
                        continue;
                    }

                    var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);

                    query = query.Where(lambda);
                }
            }

            return query;
        }

        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, QueryParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.SortBy))
                return query;

            var entityType = typeof(T);
            var property = entityType.GetProperty(parameters.SortBy,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null)
                return query; 

            var parameter = Expression.Parameter(entityType, "x");
            var propertyAccess = Expression.Property(parameter, property);
            var lambda = Expression.Lambda(propertyAccess, parameter);

            var methodName = parameters.SortDescending ? "OrderByDescending" : "OrderBy";
            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { entityType, property.PropertyType },
                query.Expression,
                Expression.Quote(lambda));

            return query.Provider.CreateQuery<T>(resultExpression);
        }

        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, QueryParameters parameters)
        {
            int skip = (parameters.Page - 1) * parameters.PageSize;
            return query.Skip(skip).Take(parameters.PageSize);
        }

        public static IQueryable<T> ApplyFilteringSortingPaging<T>(this IQueryable<T> query, QueryParameters parameters)
        {
            return query
                .ApplyFiltering(parameters)
                .ApplySorting(parameters)
                .ApplyPagination(parameters);
        }

        public static async Task<QueryResult<T>> ToQueryResultAsync<T>(
            this IQueryable<T> query,
            QueryParameters parameters)
        {
            var filteredSortedQuery = query
                .ApplyFiltering(parameters)
                .ApplySorting(parameters);

            var totalCount = await filteredSortedQuery.CountAsync();
            var pagedItems = await filteredSortedQuery
                .ApplyPagination(parameters)
                .ToListAsync();

            return new QueryResult<T>
            {
                Items = pagedItems,
                Page = parameters.Page,
                PageSize = parameters.PageSize,
                TotalCount = totalCount
            };
        }
    }
}
