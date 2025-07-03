using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace FUNewsManagement_RazorPages.Pages.NewsArticles
{
    [Authorize(Policy = "StaffOnly")]

    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IEnumerable<NewsArticle> NewsArticles { get; set; } = Enumerable.Empty<NewsArticle>();
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 4;
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public string SearchString { get; set; } = string.Empty;
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public async Task OnGetAsync(string searchString, int pageNumber = 1)
        {
            SearchString = searchString ?? string.Empty;
            CurrentPage = pageNumber < 1 ? 1 : pageNumber;
            
            var response = await _httpClient.GetFromJsonAsync<ODataResponse<NewsArticle>>("https://localhost:7130/odata/NewsArticles");
            var allArticles = response?.Value ?? new List<NewsArticle>();

            // Apply search filter
            if (!string.IsNullOrEmpty(SearchString))
            {
                allArticles = allArticles
                    .Where(a => a.NewsTitle.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                               (a.Headline != null && a.Headline.Contains(SearchString, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }

            TotalItems = allArticles.Count();
            TotalPages = (int)Math.Ceiling((double)TotalItems / PageSize);

            if (CurrentPage > TotalPages && TotalPages > 0)
            {
                CurrentPage = TotalPages;
            }
            //paging
            NewsArticles = allArticles
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

    }
}
