using app.domains.products.filters;
using app.domains.products.repository;
using app.lib.pagination;
using Microsoft.VisualBasic;
using shortid;
using shortid.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace app.domains.products.service
{
    public class ProductService(IProductRepository repo) : IProductService
    {
        public Task<List<Product>> GetAllAsync()
        {
            return repo.GetAllAsync();
        }

        public Task<Product> CreateAsync(Product product)
        {
            if (string.IsNullOrEmpty(product.Sku))
            {
                product.Sku = ShortId.Generate(new GenerationOptions(true, false, 8));
            }

            return repo.CreateAsync(product);
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            return repo.GetByIdAsync(id);
        }

        public Task<Product> UpdateAsync(int id, UpdateFields fields)
        {
            return repo.UpdateAsync(id, fields);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return repo.DeleteAsync(id);
        }

        public Task<PaginationResult<Product>> SearchAsync(Filters filters)
        {
            return repo.SearchAsync(filters);
        }
    }
}