using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement_RazorPages.Pages.SystemAccounts
{
    [Authorize(Policy = "AdminOnly")]
    public class EditPartialModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditPartialModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short id)
        {

            var systemaccount = await _httpClient.GetFromJsonAsync<SystemAccount>($"https://localhost:7130/api/SystemAccount/{id}");
            if (systemaccount == null)
            {
                return NotFound();
            }
            else
            {
                SystemAccount = systemaccount;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
                var response = await _httpClient.PutAsJsonAsync($"https://localhost:7130/api/SystemAccount/{SystemAccount.AccountId}", SystemAccount);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/SystemAccounts/Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Update failed: {response.StatusCode}");
                }

            return Page();
        }

    }
}
