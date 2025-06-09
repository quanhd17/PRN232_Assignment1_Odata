using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement_RazorPages.Pages.NewsArticles
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public DeleteModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _httpClient.DeleteAsync($"https://localhost:7130/odata/NewsArticles({id})");
            return RedirectToPage("/NewsArticles/Index");
        }
    }
}
