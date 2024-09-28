using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.ProductDto;
using efcoremongodb.Models;

namespace efcoremongodb.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();

        Task<Product?> GetProductById (string id);

        Task<Product> AddProduct(CreateProductDto newProduct);
        Task<Product?> EditProduct(string id, UpdateProductDto product);
        Task<Product?> DeleteProduct(string id);
    }
}