using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.DetailsDto.ProductDetailsDto;
using efcoremongodb.Dtos.ProductDto;
using efcoremongodb.Models;

namespace efcoremongodb.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDetailsDto>> GetAllProducts();

        Task<ProductDetailsDto?> GetProductById (string id);

        Task<ProductDetailsDto> AddProduct(CreateProductDto newProduct);
        Task<ProductDetailsDto?> EditProduct(string id, UpdateProductDto product);
        Task<Product?> DeleteProduct(string id);
    }
}