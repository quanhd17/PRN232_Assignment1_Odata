
using FUNewsManagement_RazorPages.Models;
using FUNewsManagement_RazorPages.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement_RazorPages.Pages.Tags
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel(HttpClient httpClient)
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
            Tag = tag;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine($"Sending TagId: {Tag.TagId}, TagName: {Tag.TagName}, Note: {Tag.Note}");

            if (ModelState.IsValid)
            {
                var url = $"https://localhost:7130/odata/Tags({Tag.TagId})";

                var tagDto = new TagDto
                {
                    TagId = Tag.TagId,
                    TagName = Tag.TagName,
                    Note = Tag.Note
                };

                var response = await _httpClient.PutAsJsonAsync(url, tagDto);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Cannot update tag. Server said: {errorContent}");
            }

            return Page();
        }


    }
}
