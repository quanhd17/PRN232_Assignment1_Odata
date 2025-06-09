using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FUNewsManagement_RazorPages.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ODataResponse<Category>>("https://localhost:7130/odata/Categories/Default.Active()");
            var activeCategories = response?.Value ?? new List<Category>();

            ViewData["ParentCategoryId"] = new SelectList(activeCategories, "CategoryId", "CategoryName");
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var response = await _httpClient.GetFromJsonAsync<ODataResponse<Category>>("https://localhost:7126/odata/Categories");
                var parentCategories = response?.Value ?? new List<Category>();
                ViewData["ParentCategoryId"] = new SelectList(parentCategories, "CategoryId", "CategoryName", Category.ParentCategoryId);
                return Page();
            }


            await _httpClient.PostAsJsonAsync("https://localhost:7130/odata/Categories", Category);
            return RedirectToPage("/Categories/Index");
        }

    }
}
