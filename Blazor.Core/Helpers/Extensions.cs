using CardManagement.Core.Models.Pagination;

namespace System.Linq
{
    public static class Extensions
    {
        public static PagedList<T> ToPaginatedList<T>(this IQueryable<T> collection, int pageNumber, int pageSize)
        {
            return new PagedList<T>(collection.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize);
        }
    }
}
