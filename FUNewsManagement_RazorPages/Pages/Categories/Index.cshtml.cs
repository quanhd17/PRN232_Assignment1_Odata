using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement_RazorPages.Pages.Categories
{
    [Authorize(Policy = "StaffOnly")]
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IEnumerable<Category> Category { get; set; } = Enumerable.Empty<Category>();

        public async Task OnGetAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ODataResponse<Category>>("https://localhost:7130/odata/Categories");

            Category = response?.Value ?? new List<Category>();

        }
    }
}    
