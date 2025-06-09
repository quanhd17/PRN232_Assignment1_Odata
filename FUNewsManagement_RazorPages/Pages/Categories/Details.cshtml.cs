using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement_RazorPages.Pages.Categories
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DetailsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short id)
        {

            var category = await _httpClient.GetFromJsonAsync<Category>($"https://localhost:7130/odata/Categories({id})");
            if (category == null)
            {
                return NotFound();
            }
            else
            {
                Category = category;
            }
            return Page();
        }
    }
}
