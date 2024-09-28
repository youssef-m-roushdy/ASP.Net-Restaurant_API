using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.BranchProductDto;
using efcoremongodb.Models;

namespace efcoremongodb.Mappers
{
    public static class BranchProductMapper
    {
        public static BranchProduct ToBranchProductFromDto(this BranchProductDto branchProductDto)
        {
            return new BranchProduct
            {
                ProductId = branchProductDto.ProductId,
                Quantity = branchProductDto.Quantity
            };
        }
    }
}