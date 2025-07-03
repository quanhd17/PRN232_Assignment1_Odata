using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace FUNewsManagement_RazorPages.Pages.NewsArticles
{
    [Authorize(Policy = "StaffOnly")]
    public class EditPartialModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public EditPartialModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; }

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new List<int>();

        public SelectList Categories { get; set; }
        public List<SelectListItem> Tags { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            NewsArticle = await _httpClient.GetFromJsonAsync<NewsArticle>($"https://localhost:7130/odata/NewsArticles({id})");

            if (NewsArticle == null)
            {
                return NotFound();
            }

            SelectedTagIds = NewsArticle.Tags?.Select(t => t.TagId).ToList() ?? new List<int>();

            var response = await _httpClient.GetFromJsonAsync<ODataResponse<Category>>("https://localhost:7130/odata/Categories");
            Categories = new SelectList(response.Value, "CategoryId", "CategoryName", NewsArticle.CategoryId);

            var tagResponse = await _httpClient.GetFromJsonAsync<ODataResponse<Tag>>("https://localhost:7130/odata/Tags");
            Tags = tagResponse.Value
                .Select(t => new SelectListItem { Value = t.TagId.ToString(), Text = t.TagName })
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            Console.WriteLine($"[DEBUG] ID: {id}");
            Console.WriteLine($"[DEBUG] ID: {NewsArticle.NewsArticleId}");
            Console.WriteLine($"TagIds count: {SelectedTagIds?.Count ?? 0}");
            if (int.TryParse(id, out int parsedId))
            {
                if (parsedId != NewsArticle.NewsArticleId)
                {
                    return NotFound();
                }
            }

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userIdValue) && short.TryParse(userIdValue, out short staffId))
            {
                NewsArticle.UpdatedById = staffId;
            }

            NewsArticle.ModifiedDate = DateTime.Now;
            Console.WriteLine($"[DEBUG] TagIds Count: {SelectedTagIds?.Count ?? 0}");
            Console.WriteLine($"[DEBUG] TagIds: {string.Join(", ", SelectedTagIds ?? new List<int>())}");
            var model = new
            {
                NewsArticle = NewsArticle,
                TagIds = SelectedTagIds
            };
            await _httpClient.PutAsJsonAsync($"https://localhost:7130/odata/NewsArticles({id})", model);

            return RedirectToPage("/NewsArticles/Index");
        }
    }
}
