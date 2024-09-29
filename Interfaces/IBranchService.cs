using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.BranchDto;
using efcoremongodb.Dtos.DetailsDto.BranchDetailsDto;
using efcoremongodb.Models;

namespace efcoremongodb.Interfaces
{
    public interface IBranchService
    {
        Task<IEnumerable<BranchDetailsDto>> GetAllBranchs();

        Task<BranchDetailsDto?> GetBranchById (string id);
    
        Task<BranchDetailsDto?> AddBranch(CreateBranchDto newBranch);
        Task<BranchDetailsDto?> EditBranch(string id, UpdateBranchDto Branch);
        Task<Branch?> DeleteBranch(string id);
    }
}