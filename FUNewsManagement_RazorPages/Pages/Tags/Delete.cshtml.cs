using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement_RazorPages.Pages.Tags
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DeleteModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Tag Tag { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _httpClient.GetFromJsonAsync<Tag>($"https://localhost:7130/odata/Tags({id})");

            if (tag == null)
            {
                return NotFound();
            }
            else
            {
                Tag = tag;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {

            await _httpClient.DeleteAsync($"https://localhost:7130/odata/Tags({id})");


            return RedirectToPage("./Index");
        }
    }
}
