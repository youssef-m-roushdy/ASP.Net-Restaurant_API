using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace efcoremongodb.Dtos.DetailsDto.UserDetailsDto
{
    public class UserDetailsDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public List<CommentUserDto> Comments { get; set; }
        public List<OrderUserDto> Orders { get; set; }
    }
}