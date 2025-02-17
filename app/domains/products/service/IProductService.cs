using app.domains.products.filters;
using app.lib.pagination;

namespace app.domains.products.service
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> CreateAsync(Product product);
        Task<Product?> GetByIdAsync(int id);
        Task<Product> UpdateAsync(int id, UpdateFields fields);
        Task<bool> DeleteAsync(int id);
        Task<PaginationResult<Product>> SearchAsync(Filters filters);
    }
}
