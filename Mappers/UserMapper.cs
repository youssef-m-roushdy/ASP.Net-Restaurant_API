using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Dtos.UserDto;
using efcoremongodb.Models;

namespace efcoremongodb.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this ApplicationUser user)
        {
            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                CommentIds = user.CommentIds,
                OrderIds = user.OrderIds
            };
        }
    }
}