using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.BranchDto;
using efcoremongodb.Models;

namespace efcoremongodb.Mappers
{
    public static class BranchMapper
    {
        public static Branch ToBranchFromCreateDto(this CreateBranchDto branch)
        {
            
            return new Branch
            {
                Name = branch.Name,
                Address = branch.Address,
                ContactNumber = branch.ContactNumber,
                OpeningHours = branch.OpeningHours
            };
        }
    }
}