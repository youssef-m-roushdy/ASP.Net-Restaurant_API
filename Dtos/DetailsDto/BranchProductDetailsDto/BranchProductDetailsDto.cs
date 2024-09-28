using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Models;

namespace efcoremongodb.Dtos.DetailsDto.BranchProductDetailsDto
{
    public class BranchProductDetailsDto
    {
        public string Id { get; set; }
        public BranchInfoDto Branch { get; set; }
        public ProductInfoDto Product { get; set; }
        public decimal Quantity { get; set; }
    }
}