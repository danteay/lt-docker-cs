using Microsoft.EntityFrameworkCore;

namespace app.lib.pagination
{
    public struct PaginationMetadata
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }

    public struct PaginationResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public PaginationMetadata Metadata { get; set; }
    }

    public class Pagination<T>(IQueryable<T> query, IFilters<T> filters)
    {
        private IFilters<T> Filters { get; set; } = filters;
        private IQueryable<T> Query { get; set; } = query;

        public async Task<PaginationResult<T>> GetResultAsync()
        {
            var query = Filters.Apply(Query);
            var total = await query.CountAsync();
            var pageSize =
                Filters.PageSize() > Filters.MaxPageSize()
                    ? Filters.MaxPageSize()
                    : Filters.PageSize();
            var totalPages = (int)Math.Ceiling(total / (double)pageSize);
            var currentPage = Filters.PageNumber() < 1 ? 1 : Filters.PageNumber();
            currentPage = currentPage > totalPages ? totalPages : currentPage;

            var offset = (currentPage - 1) * pageSize;
            if (offset < 0)
                offset = 0;

            var items = await Filters.Apply(Query).Skip(offset).Take(pageSize).ToListAsync();

            return new PaginationResult<T>
            {
                Items = items,
                Metadata = new PaginationMetadata
                {
                    PageNumber = currentPage,
                    PageSize = pageSize,
                    TotalCount = total,
                    TotalPages = totalPages,
                },
            };
        }
    }

    public abstract class TransformResult<T, TU>
    {
        public static PaginationResult<TU> Apply(PaginationResult<T> got, Func<T, TU> selectFunc)
        {
            return new PaginationResult<TU>
            {
                Items = got.Items.Select(selectFunc),
                Metadata = got.Metadata,
            };
        }
    }
}
