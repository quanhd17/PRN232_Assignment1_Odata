using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FUNewsManagement_RazorPages.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _httpClient.GetFromJsonAsync<Category>($"https://localhost:7130/odata/Categories({id})");
            if (category == null)
            {
                return NotFound();
            }
            Category = category;
            var response = await _httpClient.GetFromJsonAsync<ODataResponse<Category>>("https://localhost:7130/odata/Categories/Default.Active()");
            var activeCategories = response?.Value ?? new List<Category>();

            ViewData["ParentCategoryId"] = new SelectList(activeCategories, "CategoryId", "CategoryName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var url = $"https://localhost:7130/odata/Categories({Category.CategoryId})";
            var response = await _httpClient.PutAsJsonAsync(url, Category);

            return RedirectToPage("./Index");
        }
    }
}
