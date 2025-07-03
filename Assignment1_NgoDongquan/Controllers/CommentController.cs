using BusinessLogic.Service;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.Cors;

namespace Assignment1_NgoDongquan.Controllers
{
    [EnableCors("AllowAll")]
    public class CommentsController : ODataController
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // GET odata/Comments
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var comments = await _commentService.GetAllCommentsAsync();
            return Ok(comments.AsQueryable());
        }

        // GET odata/Comments(1)
        [EnableQuery]
        public async Task<IActionResult> Get([FromRoute] int key)
        {
            var comment = await _commentService.GetCommentByIdAsync(key);
            if (comment == null) return NotFound();
            return Ok(comment);
        }

        // POST odata/Comments
        public async Task<IActionResult> Post([FromBody] Comment comment)
        {
            if (comment == null)
                return BadRequest("Comment data is required.");

            try
            {
                var createdComment = await _commentService.AddCommentAsync(comment);
                return Created(createdComment);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating comment: {ex.Message}");
            }
        }

        // PUT odata/Comments(1)
        public async Task<IActionResult> Put([FromRoute] int key, [FromBody] Comment comment)
        {
            if (comment == null || comment.CommentId != key)
                return BadRequest("Invalid comment data or mismatched ID.");

            try
            {
                var existingComment = await _commentService.GetCommentByIdAsync(key);
                if (existingComment == null)
                    return NotFound();

                // Update only the content
                existingComment.Content = comment.Content;
                
                // Update the comment (you'll need to add UpdateCommentAsync to the service)
                await _commentService.UpdateCommentAsync(existingComment);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating comment: {ex.Message}");
            }
        }

        // DELETE odata/Comments(1)
        public async Task<IActionResult> Delete([FromRoute] int key)
        {
            try
            {
                await _commentService.DeleteCommentAsync(key);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting comment: {ex.Message}");
            }
        }

        // GET odata/Comments/GetByArticleId(articleId=1)
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetByArticleId([FromODataUri] int articleId)
        {
            var comments = await _commentService.GetCommentsByArticleIdAsync(articleId);
            return Ok(comments.AsQueryable());
        }
    }
} 