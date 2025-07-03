using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FUNewsManagement_RazorPages.Pages.Tags
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Tag Tag { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null 
                };
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7130/odata/Tags", Tag, options);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Cannot create tag. Server said: {errorContent}");
                var json = JsonSerializer.Serialize(Tag, options);
                Console.WriteLine("Sending Tag JSON: " + json);

            }

            return Page();
        }

    }
}
