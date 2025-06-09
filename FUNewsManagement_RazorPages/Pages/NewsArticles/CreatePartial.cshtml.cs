using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace FUNewsManagement_RazorPages.Pages.NewsArticles
{
    public class CreatePartialModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty]
        public NewsArticle NewsArticle { get; set; }

        [BindProperty]
        public List<int> TagIds { get; set; } = new();

        public SelectList CategoryList { get; set; }
        public MultiSelectList TagList { get; set; }

        public CreatePartialModel(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        [Authorize(Policy = "StaffOnly")]
        public async Task<IActionResult> OnGetAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ODataResponse<Category>>("https://localhost:7130/odata/Categories/Default.Active()");
            CategoryList = new SelectList(response?.Value ?? new List<Category>(), "CategoryId", "CategoryName");

            var tagResponse = await _httpClient.GetFromJsonAsync<ODataResponse<Tag>>("https://localhost:7130/odata/Tags");
            TagList = new MultiSelectList(tagResponse?.Value ?? new List<Tag>(), "TagId", "TagName");


            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userIdValue) && short.TryParse(userIdValue, out short staffId))
            {
                NewsArticle.CreatedById = staffId;
                NewsArticle.UpdatedById = staffId;
            }

            NewsArticle.CreatedDate = DateTime.Now;
            NewsArticle.ModifiedDate = DateTime.Now;

            var model = new
            {
                NewsArticle = NewsArticle,
                TagIds = TagIds
            };
            await _httpClient.PostAsJsonAsync("https://localhost:7130/odata/NewsArticles", model);
            return RedirectToPage("/NewsArticles/Index");
        }


    }
}
