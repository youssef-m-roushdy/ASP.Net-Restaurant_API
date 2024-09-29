using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace efcoremongodb.Dtos.DetailsDto.BranchDetailsDto
{
    public class BranchDetailsDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string OpeningHours { get; set; }
        public List<OrdersInBranchDto> Orders { get; set; }
        public List<BranchInfoProductDto> BranchProducts { get; set; }
    }
}