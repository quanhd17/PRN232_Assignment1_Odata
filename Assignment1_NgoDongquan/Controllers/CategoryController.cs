using BusinessLogic.Service;
using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Assignment1_NgoDongquan.Controllers
{
    public class CategoriesController : ODataController
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: odata/Categories
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories.AsQueryable());
        }

        // GET: odata/Categories(1)
        [EnableQuery]
        public async Task<IActionResult> Get([FromRoute] short key)
        {
            var category = await _categoryService.GetCategoryByIdAsync(key);
            if (category == null) return NotFound();
            return Ok(category);
        }

        // POST: odata/Categories
        public async Task<IActionResult> Post([FromBody] Category category)
        {
            if (category == null) return BadRequest("Category cannot be null.");
            await _categoryService.AddCategoryAsync(category);
            return Created(category);
        }

        // PUT: odata/Categories(1)
        public async Task<IActionResult> Put([FromRoute] short key, [FromBody] Category category)
        {
            if (category == null || category.CategoryId != key)
                return BadRequest("Mismatched category ID.");

            await _categoryService.UpdateCategoryAsync(category);
            return NoContent();
        }

        // DELETE: odata/Categories(1)
        public async Task<IActionResult> Delete([FromRoute] short key)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(key);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        // GET: odata/Categories/Active
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Active()
        {
            var activeCategories = await _categoryService.GetActiveCategoriesAsync();
            return Ok(activeCategories.AsQueryable());
        }
    }
}
