using BusinessLogic.Service;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Formatter;

namespace Assignment1_NgoDongquan.Controllers
{
    public class TagsController : ODataController
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // GET: odata/Tags
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return Ok(tags.AsQueryable());
        }

        // GET: odata/Tags(1)
        [EnableQuery]
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var tag = await _tagService.GetTagByIdAsync(key);
            if (tag == null) return NotFound();
            return Ok(tag);
        }

        // POST: odata/Tags
        public async Task<IActionResult> Post([FromBody] Tag tag)
        {
            if (tag == null)
                return BadRequest("Tag cannot be null.");

            await _tagService.AddTagAsync(tag);
            return Created(tag);
        }

        // PUT: odata/Tags(1)
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Tag tag)
        {
            if (tag == null || tag.TagId != key)
                return BadRequest("Mismatched tag ID.");

            await _tagService.UpdateTagAsync(tag);
            return NoContent();
        }

        // DELETE: odata/Tags(1)
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var tag = await _tagService.GetTagByIdAsync(key);
            if (tag == null)
                return NotFound();

            await _tagService.DeleteTagAsync(key);
            return NoContent();
        }
    }
}
