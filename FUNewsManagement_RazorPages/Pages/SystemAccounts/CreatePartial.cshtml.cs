using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement_RazorPages.Pages.SystemAccounts
{
    [Authorize(Policy = "AdminOnly")]
    public class CreatePartialModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = new();

        public CreatePartialModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var existingAccounts = await _httpClient.PostAsJsonAsync("https://localhost:7130/odata/SystemAccounts/Default.Any", SystemAccount);

            if (existingAccounts.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Account already exist";
                return RedirectToPage("./Index");
            }

            await _httpClient.PostAsJsonAsync("https://localhost:7130/odata/SystemAccounts", SystemAccount);
            return RedirectToPage("./Index");
        }
    }
}
