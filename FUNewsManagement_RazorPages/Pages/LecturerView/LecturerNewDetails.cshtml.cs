using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement_RazorPages.Pages.LecturerView
{
    public class LecturerNewDetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public LecturerNewDetailsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public NewsArticle? NewsArticle { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                // Load categories first
                await LoadCategories();
                
                // Fetch all news articles and find the one with matching ID
                var response = await _httpClient.GetFromJsonAsync<ODataResponse<NewsArticle>>("https://localhost:7130/odata/NewsArticles");
                
                if (response?.Value != null)
                {
                    NewsArticle = response.Value.FirstOrDefault(a => a.NewsArticleId == id);
                    
                    if (NewsArticle != null)
                    { 
                        return Page();
                    }
                }

                return NotFound();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching news article: {ex.Message}");
                return StatusCode(500, "Unable to fetch the news article. Please try again later.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");             
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
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