using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.DetailsDto.UserDetailsDto;
using efcoremongodb.Dtos.UserDto;
using efcoremongodb.Models;

namespace efcoremongodb.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser?> DeleteUserAsync(Guid userId);
        Task<UserDetailsDto?> GetUserProfile(Guid userId);
    }
}