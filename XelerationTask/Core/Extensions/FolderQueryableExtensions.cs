using XelerationTask.Core.Models;

namespace XelerationTask.Core.Extensions
{
    public static class FolderQueryableExtensions
    {
        public static IQueryable<ProjectFolder> ApplyFiltering(this IQueryable<ProjectFolder> query, QueryParameters parameters)
        {
            if (!string.IsNullOrEmpty(parameters.NameContains))
                query = query.Where(f => f.Name.Contains(parameters.NameContains));

            return query;
        }

        public static IQueryable<ProjectFolder> ApplySorting(this IQueryable<ProjectFolder> query, QueryParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.SortBy))
                return query;

            switch (parameters.SortBy.ToLower())
            {
                case "Name":
                    query = parameters.SortDescending
                        ? query.OrderByDescending(f => f.Name)
                        : query.OrderBy(f => f.Name);
                    break;

                case "CreatedAt":
                    query = parameters.SortDescending
                        ? query.OrderByDescending(f => f.CreatedAt)
                        : query.OrderBy(f => f.CreatedAt);
                    break;

                case "UpdatedAt":
                    query = parameters.SortDescending
                        ? query.OrderByDescending(f => f.UpdatedAt)
                        : query.OrderBy(f => f.UpdatedAt);
                    break;

                default:
                    break;
            }

            return query;
        }


        public static IQueryable<ProjectFolder> ApplyPagination(this IQueryable<ProjectFolder> query, QueryParameters parameters)
        {
            int skip = (parameters.Page - 1) * parameters.PageSize;
            return query.Skip(skip).Take(parameters.PageSize);
        }

        public static IQueryable<ProjectFolder> ApplyFilteringSortingPaging(this IQueryable<ProjectFolder> query, QueryParameters parameters)
        {
            return query
                .ApplyFiltering(parameters)
                .ApplySorting(parameters)
                .ApplyPagination(parameters);
        }
    }
}
