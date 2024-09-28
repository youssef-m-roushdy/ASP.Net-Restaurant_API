using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace efcoremongodb.Dtos.OrderDto
{
    public class ConfirmOrderDto
    {
        [Required]
        public string BranchId { get; set; }
        [Required]
        [AllowedValues("Cash", "Visa", "Anyother")]
        public string PayType { get; set; }
    }
}