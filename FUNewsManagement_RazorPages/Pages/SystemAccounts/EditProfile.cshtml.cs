using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace FUNewsManagement_RazorPages.Pages.SystemAccounts
{
    [Authorize]
    public class EditProfileModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty]
        public SystemAccount SystemAccount { get; set; }

        [BindProperty]
        public string OldPassword { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        public EditProfileModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !short.TryParse(userIdClaim, out short userId))
            {
                return Unauthorized();
            }

            var account = await _httpClient.GetFromJsonAsync<SystemAccount>($"https://localhost:7126/api/SystemAccount/{userId}");
            if (account == null) return NotFound();

            SystemAccount = account;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !short.TryParse(userIdClaim, out short userId))
            {
                return Unauthorized();
            }

            if (userId != SystemAccount.AccountId) return Forbid();

            var existingAccount = await _httpClient.GetFromJsonAsync<SystemAccount>($"https://localhost:7126/api/SystemAccount/{userId}");
            if (existingAccount == null) return NotFound();

            // check if user old password is correct
            if (!string.IsNullOrEmpty(OldPassword) && !string.IsNullOrEmpty(NewPassword))
            {
                if (existingAccount.AccountPassword != OldPassword)
                {
                    TempData["ErrorMessage"] = "Old password is incorrect!";
                    return Page();
                }
                existingAccount.AccountPassword = NewPassword;
            }

            existingAccount.AccountName = SystemAccount.AccountName;

            await _httpClient.PutAsJsonAsync($"https://localhost:7130/api/SystemAccount", existingAccount);
            TempData["SuccessMessage"] = "Profile updated successfully!";

            return RedirectToPage("/SystemAccounts/EditProfile");
        }
    }
}
