using System.Data;
using System.Security.Claims;
using FUNewsManagement_RazorPages.Models;
using FUNewsManagement_RazorPages.Models.Dto;
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

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7130/odata/SystemAccounts/Default.Login", loginModel);

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginDto>();

                var role = loginResponse.AccountRole switch
                {
                    1 => "Staff",
                    2 => "Lecturer",
                    0 => "Admin",
                    _ => "Lecturer"
                };

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginResponse.AccountName),
                    new Claim(ClaimTypes.Email, Input.Email),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("AccessToken", loginResponse.Token)
                };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", principal);

                return role switch
                {
                    "Staff" => RedirectToPage("/NewsArticles/Index"),
                    "Lecturer" => RedirectToPage("/LecturerView/LecturerNews"),
                    "Admin" => RedirectToPage("/SystemAccounts/Index"),
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