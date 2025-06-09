using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement_RazorPages.Pages.NewsArticles
{
    public class LecturerNewsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public LecturerNewsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }
        public IEnumerable<NewsArticle> NewsArticles { get; set; } = Enumerable.Empty<NewsArticle>();


        public async Task OnGetAsync(string searchString)
        {
            var response = await _httpClient.GetFromJsonAsync<ODataResponse<NewsArticle>>("https://localhost:7130/odata/NewsArticle");

            NewsArticles = response?.Value ?? new List<NewsArticle>();

            if (!string.IsNullOrEmpty(searchString))
            {
                NewsArticles = NewsArticles
                    .Where(a => a.NewsTitle.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

    }
}
