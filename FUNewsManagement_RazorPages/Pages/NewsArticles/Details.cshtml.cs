using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement_RazorPages.Pages.NewsArticles
{
    [Authorize(Policy = "StaffOnly")]

    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DetailsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public NewsArticle NewsArticle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var article = await _httpClient.GetFromJsonAsync<NewsArticle>($"https://localhost:7130/odata/NewsArticle({id})");
            if (article == null)
            {
                return NotFound();
            }
            else
            {
                NewsArticle = article;
                NewsArticle.Category = await _httpClient.GetFromJsonAsync<Category>($"https://localhost:7130/odata/Categories({id})");
                if (NewsArticle.Tags != null && NewsArticle.Tags.Count > 0)
                {
                    foreach (var tag in NewsArticle.Tags)
                    {
                        tag.NewsArticles = null;
                    }
                }
            }
            return Page();
        }
    }
}
