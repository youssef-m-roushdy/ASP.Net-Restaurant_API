using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.BranchDto;
using efcoremongodb.Models;

namespace efcoremongodb.Interfaces
{
    public interface IBranchService
    {
        Task<IEnumerable<Branch>> GetAllBranchs();

        Task<Branch?> GetBranchById (string id);
    
        Task<Branch?> AddBranch(CreateBranchDto newBranch);
        Task<Branch?> EditBranch(string id, UpdateBranchDto Branch);
        Task<Branch?> DeleteBranch(string id);
    }
}