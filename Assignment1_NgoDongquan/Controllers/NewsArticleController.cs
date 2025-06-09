using BusinessLogic.Service;
using BusinessObject.Dto;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Assignment1_NgoDongquan.Controllers
{
    public class NewsArticleController : ODataController
    {
        private readonly INewsArticleService _newsArticleService;

        public NewsArticleController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        // GET odata/NewsArticles
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var articles = await _newsArticleService.GetAllNewsArticlesAsync();
            return Ok(articles.AsQueryable());
        }

        // GET odata/NewsArticles(1)
        [EnableQuery]
        public async Task<IActionResult> Get([FromRoute] int key)
        {
            var article = await _newsArticleService.GetNewsArticleByIdAsync(key);
            if (article == null) return NotFound();
            return Ok(article);
        }

        // POST odata/NewsArticles
        public async Task<IActionResult> Post([FromBody] NewArticleDto model)
        {
            if (model.NewsArticle == null || model.TagIds == null || !model.TagIds.Any())
                return BadRequest("Invalid news article or tag IDs.");

            await _newsArticleService.AddNewsArticleAsync(model.NewsArticle, model.TagIds);
            return Created(model.NewsArticle);
        }

        // PUT odata/NewsArticles(1)
        public async Task<IActionResult> Put([FromRoute] int key, [FromBody] NewArticleDto model)
        {
            if (model.NewsArticle == null || model.NewsArticle.NewsArticleId != key)
                return BadRequest("Mismatched article ID.");

            await _newsArticleService.UpdateNewsArticleAsync(model.NewsArticle, model.TagIds);
            return NoContent();
        }

        // DELETE odata/NewsArticles(1)
        public async Task<IActionResult> Delete([FromRoute] int key)
        {
            var article = await _newsArticleService.GetNewsArticleByIdAsync(key);
            if (article == null) return NotFound();

            await _newsArticleService.DeleteNewsArticleAsync(key);
            return NoContent();
        }

        // GET odata/NewsArticles/GetByStaffId(staffId=1)
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetByStaffId([FromODataUri] short staffId)
        {
            var articles = await _newsArticleService.GetNewsArticleByStaffIdAsync(staffId);
            return Ok(articles.AsQueryable());
        }

        // GET odata/NewsArticles/GetActiveNews
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetActiveNews()
        {
            var activeArticles = await _newsArticleService.GetActiveNewsForLecturerAsync();
            return Ok(activeArticles.AsQueryable());
        }
    }
}
