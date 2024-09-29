using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace efcoremongodb.Dtos.DetailsDto.BranchDetailsDto
{
    public class BranchInfoProductDto
    {
        public string Id { get; set; }
        public string Product { get; set; }
        public decimal Quantity { get; set; }
    }
}