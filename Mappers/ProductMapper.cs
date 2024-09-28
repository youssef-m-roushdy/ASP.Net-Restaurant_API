using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.ProductDto;
using efcoremongodb.Models;

namespace efcoremongodb.Mappers
{
    public static class ProductMapper
    {
        public static Product ToProductFromCreateDto(this CreateProductDto product)
        {
            
            return new Product
            {
                Image = product.Image,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
            };
        }

        
    }
}