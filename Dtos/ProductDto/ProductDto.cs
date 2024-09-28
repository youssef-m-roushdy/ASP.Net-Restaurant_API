using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace efcoremongodb.Dtos.ProductDto
{
    public class ProductDto
    {
        public string ProductId { get; set; }   // Product ID
        public string Name { get; set; }        // Product Name
        public decimal Price { get; set; }      // Product Price
        public string Description { get; set; } // Product Description (optional)
    }
}