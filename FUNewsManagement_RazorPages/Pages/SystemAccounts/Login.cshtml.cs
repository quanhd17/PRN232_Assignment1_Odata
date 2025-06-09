using System.Security.Claims;
using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement_RazorPages.Pages.SystemAccounts
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public LoginModel(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; } = new();

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var defaultAdminEmail = _config["DefaultAccount:Email"];
            var defaultAdminPassword = _config["DefaultAccount:Password"];

            if (Input.Email == defaultAdminEmail && Input.Password == defaultAdminPassword)
            {
                var adminClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Admin"),
                    new Claim(ClaimTypes.Email, defaultAdminEmail),
                    new Claim(ClaimTypes.Role, "Admin")
                };
                var adminIdentity = new ClaimsIdentity(adminClaims, "MyCookieAuth");
                var adminrincipal = new ClaimsPrincipal(adminIdentity);
                await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(adminIdentity));

                return RedirectToPage("/SystemAccounts/Index");
            }

            var loginModel = new
            {
                AccountEmail = Input.Email,
                AccountPassword = Input.Password
            };
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7130/api/SystemAccount/login", loginModel);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize user data from response
                var user = await response.Content.ReadFromJsonAsync<SystemAccount>();

                var role = user.AccountRole switch
                {
                    1 => "Staff",
                    2 => "Lecturer",
                    _ => "Lecturer"
                };

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                    new Claim(ClaimTypes.Name, user.AccountName ?? ""),
                    new Claim(ClaimTypes.Email, user.AccountEmail ?? ""),
                    new Claim(ClaimTypes.Role, role)
                };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", principal);

                return role switch
                {
                    "Staff" => RedirectToPage("/NewsArticles/Index"),
                    "Lecturer" => RedirectToPage("/NewsArticles/LecturerNews"),
                    _ => RedirectToPage("/NewsArticles/LecturerNews")
                };
            }

            ErrorMessage = "Invalid credentials!";
            return Page();
        }

        public class LoginInputModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}