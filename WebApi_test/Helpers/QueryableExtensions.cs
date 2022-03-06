using System.Linq;
using WebApi_test.DTO;

namespace WebApi_test.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO pagination)
        {
            return queryable
                .Skip((pagination.Page - 1) * pagination.RecordPerPage)
                .Take(pagination.RecordPerPage);
        }
    }
}
