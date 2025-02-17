using app.domains.products.filters;
using app.infra;
using app.lib.pagination;
using Microsoft.EntityFrameworkCore;

namespace app.domains.products.repository
{
    public class ProductNotFoundException(string message) : Exception(message);

    public class ProductRepository(PostgresDbContext dbContext) : IProductRepository
    {
        public Task<List<Product>> GetAllAsync()
        {
            return dbContext.Products.ToListAsync();
        }

        public async Task<Product> CreateAsync(Product product)
        {
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var product = await dbContext.Products.FindAsync(id);
            if (product == null)
                throw new ProductNotFoundException($"Product with id {id} not found");

            return product;
        }

        public async Task<Product> UpdateAsync(int id, UpdateFields fields)
        {
            var product = await dbContext.Products.FindAsync(id);
            if (product == null)
                throw new ProductNotFoundException($"Product with id {id} not found");

            fields.Apply(ref product);

            await dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await dbContext.Products.FindAsync(id);
            if (product == null)
                throw new ProductNotFoundException($"Product with id {id} not found");

            dbContext.Products.Remove(product);

            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<PaginationResult<Product>> SearchAsync(Filters filters)
        {
            return await new Pagination<Product>(dbContext.Products, filters).GetResultAsync();
        }
    }
}
