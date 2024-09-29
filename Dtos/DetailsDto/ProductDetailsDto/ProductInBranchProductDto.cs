using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace efcoremongodb.Dtos.DetailsDto.ProductDetailsDto
{
    public class ProductInBranchProductDto
    {
        public string Id { get; set; }
        public string Branch { get; set; }
        public decimal Quantity { get; set; }
    }
}