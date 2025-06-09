using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement_RazorPages.Pages.SystemAccounts
{
    [Authorize(Policy = "AdminOnly")]
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DeleteModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> OnPostAsync(short id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7130/api/SystemAccount/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/SystemAccounts/Index");
            }
            var error = await response.Content.ReadAsStringAsync();
            return BadRequest(error);
        }
    }
}
