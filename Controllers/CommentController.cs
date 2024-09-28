using System.Security.Claims;
using System.Threading.Tasks;
using efcoremongodb.Dtos.CommentDto;
using efcoremongodb.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace efcoremongodb.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetCommentsOnProduct(string productId)
        {
            var comments = await _commentService.GetCommentsOnProduct(productId);
            return Ok(comments);
        }

        [HttpPost("{productId}")]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromRoute]string productId,[FromBody]CreateCommentDto comment)
        {
            if (!ModelState.IsValid)
                return BadRequest("Order cannot be null.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            var userId = userIdClaim.Value;

            var createdComment = await _commentService.AddComment(productId, userId, comment);
            if(createdComment == null)
                return BadRequest("Can not insert that comment");
            return Ok(createdComment);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute]string id)
        {
            var deletedComment = await _commentService.DeleteComment(id);
            if(deletedComment == null)
                return NotFound("Comment not found");
            return NoContent();
        }
    }
}
