using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace efcoremongodb.Dtos.DetailsDto.ProductDetailsDto
{
    public class ProductDetailsDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public List<CommentOnProductDto> Comments { get; set; }
        public List<ProductInBranchProductDto> BranchProducts { get; set; }
    }
}