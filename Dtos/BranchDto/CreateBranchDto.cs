using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace efcoremongodb.Dtos.BranchDto
{
    public class CreateBranchDto
    {
        [Required(ErrorMessage = "You must provide a branch name.")]
        [StringLength(100, ErrorMessage = "Branch name cannot be longer than 100 characters.")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "You must provide the address of the branch.")]
        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public string Address { get; set; }
        [Required]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string ContactNumber { get; set; }
        [Required(ErrorMessage = "you have insert branch opening hours")]
        public string OpeningHours { get; set; }
    }
}