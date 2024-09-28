using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using efcoremongodb.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace efcoremongodb.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var userProfile = await _userService.GetUserProfile(id);
            if(userProfile == null)
                return NotFound("user id is invalid");
            return Ok(userProfile);
        }

        [HttpDelete("{id}/delete-account")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            var deletedUser = await _userService.DeleteUserAsync(id);
            if(deletedUser == null)
                return NotFound("user id is invalid");
            return NoContent();
        }
    }
}