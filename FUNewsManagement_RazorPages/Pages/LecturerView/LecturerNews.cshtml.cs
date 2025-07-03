using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement_RazorPages.Pages.LecturerView
{
    public class LecturerNewsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public LecturerNewsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }
        public IEnumerable<NewsArticle> NewsArticles { get; set; } = Enumerable.Empty<NewsArticle>();
        public List<Category> Categories { get; set; } = new List<Category>();


        public async Task OnGetAsync(string searchString)
        {
            // Load categories first
            await LoadCategories();
            
            var response = await _httpClient.GetFromJsonAsync<ODataResponse<NewsArticle>>("https://localhost:7130/odata/NewsArticles");

            NewsArticles = response?.Value ?? new List<NewsArticle>();

            if (!string.IsNullOrEmpty(searchString))
            {
                NewsArticles = NewsArticles
                    .Where(a => a.NewsTitle.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        private async Task LoadCategories()
        {
            try
            {
                var categoriesResponse = await _httpClient.GetFromJsonAsync<ODataResponse<Category>>("https://localhost:7130/odata/Categories");
                if (categoriesResponse?.Value != null)
                {
                    Categories = categoriesResponse.Value.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading categories: {ex.Message}");
                // Don't fail the page if categories fail to load
            }
        }

    }
}
