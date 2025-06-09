using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement_RazorPages.Pages.Tags
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IEnumerable<Tag> Tags { get; set; } = Enumerable.Empty<Tag>();

        public async Task OnGetAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ODataResponse<Tag>>("https://localhost:7130/odata/Tags");
            Tags = response?.Value ?? new List<Tag>();
        }
    }
}
