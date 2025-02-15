using app.controllers.dtos;
using app.controllers.dtos.products;
using app.domains.products;
using app.domains.products.filters;
using app.domains.products.repository;
using app.domains.products.service;
using app.lib.pagination;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace app.controllers
{
    [Route("/api/products")]
    [ApiController]
    public class ProductsController(IProductService srvProducts) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<ActionResult<GetAllResponse>> GetAll()
        {
            var products = await srvProducts.GetAllAsync();
            var response = new GetAllResponse
            {
                Items = products.Select(p => new ProductItem
                {
                    Id = p.Id,
                    Name = p.Name,
                    Sku = p.Sku,
                    Price = p.Price
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductItem>> GetById(int id)
        {
            try
            {
                var product = await srvProducts.GetByIdAsync(id);
                if (product == null)
                    return NotFound();

                var response = new ProductItem
                {
                    Id = product.Id,
                    Name = product.Name,
                    Sku = product.Sku,
                    Price = product.Price
                };

                return Ok(response);
            }
            catch (ProductNotFoundException e)
            {
                return NotFound(new ErrorResponse(message: e.Message));
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: " + e.Message);
                return StatusCode(500, new ErrorResponse(message: "Unexpected error"));
            }
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResult<ProductItem>>> Search(
            [FromQuery] Filters filters,
            [FromQuery] int limit = 10,
            [FromQuery] int page = 1)
        {
            var result = await srvProducts.SearchAsync(filters.SetPageSize(limit).SetPageNumber(page));

            var transformed = TransformResult<Product, ProductItem>.Apply(result, p => new ProductItem
            {
                Id = p.Id,
                Name = p.Name,
                Sku = p.Sku,
                Price = p.Price
            });

            return Ok(transformed);
        }

        [HttpPost]
        public async Task<ActionResult<ProductItem>> Create([FromBody] CreateRequest request)
        {
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Sku = ""
            };

            var created = await srvProducts.CreateAsync(product);
            var response = new ProductItem
            {
                Id = created.Id,
                Name = created.Name,
                Sku = created.Sku,
                Price = created.Price
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductItem>> Update(int id, [FromBody] UpdateFields request)
        {
            try
            {
                var updated = await srvProducts.UpdateAsync(id, request);

                var response = new ProductItem
                {
                    Id = updated.Id,
                    Name = updated.Name,
                    Sku = updated.Sku,
                    Price = updated.Price
                };

                return Ok(response);
            }
            catch (ProductNotFoundException e)
            {
                return NotFound(new ErrorResponse(message: e.Message));
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: " + e.Message);
                return StatusCode(500, new ErrorResponse(message: "Unexpected error"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteResponse>> Delete(int id)
        {
            try
            {
                await srvProducts.DeleteAsync(id);

                var response = new DeleteResponse
                {
                    Id = id,
                    Deleted = true
                };

                return Ok(response);
            }
            catch (ProductNotFoundException e)
            {
                return NotFound(new ErrorResponse(message: e.Message));
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: " + e.Message);
                return StatusCode(500, new ErrorResponse(message: "Unexpected error"));
            }
        }
    }
}