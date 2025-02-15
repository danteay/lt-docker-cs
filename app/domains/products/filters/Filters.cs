using app.lib.pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq;

// ReSharper disable InconsistentNaming

namespace app.domains.products.filters
{
    public class Filters : IFilters<Product>
    {
        private const int _maxPageSize = 100;
        private int _pageNumber { get; set; } = 1;
        private int _pageSize { get; set; } = 10;

        public int? Id { get; set; }
        public string? Sku { get; set; }
        public string? SearchTerm { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public IQueryable<Product> Apply(IQueryable<Product> query)
        {
            if (Id.HasValue) query = query.Where(p => p.Id == Id.Value);

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(p =>
                    EF.Functions.ILike(p.Name, $"%{SearchTerm}%") ||
                    EF.Functions.ILike(p.Sku, $"%{SearchTerm}%"));
            }

            if (!string.IsNullOrEmpty(Sku)) query = query.Where(p => p.Sku == Sku);

            if (MinPrice.HasValue) query = query.Where(p => p.Price >= MinPrice.Value);

            if (MaxPrice.HasValue) query = query.Where(p => p.Price <= MaxPrice.Value);

            return query.OrderBy(p => p.Id);
        }

        public int MaxPageSize()
        {
            return _maxPageSize;
        }

        public int PageSize()
        {
            return _pageSize;
        }

        public int PageNumber()
        {
            return _pageNumber;
        }

        public Filters SetPageNumber(int pageNumber)
        {
            _pageNumber = pageNumber;
            return this;
        }

        public Filters SetPageSize(int pageSize)
        {
            _pageSize = pageSize;
            return this;
        }
    }
}