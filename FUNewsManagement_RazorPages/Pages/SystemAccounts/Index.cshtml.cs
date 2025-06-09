using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement_RazorPages.Pages.SystemAccounts
{
    [Authorize(Policy = "AdminOnly")]
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IEnumerable<SystemAccount> Accounts { get; set; } = Enumerable.Empty<SystemAccount>();

        public async Task OnGetAsync()
        {
            Accounts = await _httpClient.GetFromJsonAsync<List<SystemAccount>>("https://localhost:7130/api/SystemAccount") ?? new List<SystemAccount>();

        }

    }
}
