using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace FUNewsManagement_RazorPages.Pages.SystemAccounts
{
    [Authorize(Policy = "AdminOnly")]
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IEnumerable<SystemAccount> Accounts { get; set; } = Enumerable.Empty<SystemAccount>();
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 4;
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public string SearchString { get; set; } = string.Empty;
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public async Task OnGetAsync(string searchString, int pageNumber = 1)
        {
            SearchString = searchString ?? string.Empty;
            CurrentPage = pageNumber < 1 ? 1 : pageNumber;
            
            var response = await _httpClient.GetFromJsonAsync<ODataResponse<SystemAccount>>("https://localhost:7130/odata/SystemAccounts");
            var allAccounts = response?.Value ?? new List<SystemAccount>();

            // Apply search filter
            if (!string.IsNullOrEmpty(SearchString))
            {
                allAccounts = allAccounts
                    .Where(a => a.AccountName.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                               (a.AccountEmail != null && a.AccountEmail.Contains(SearchString, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }

            TotalItems = allAccounts.Count();
            TotalPages = (int)Math.Ceiling((double)TotalItems / PageSize);

            // Ensure current page is within valid range
            if (CurrentPage > TotalPages && TotalPages > 0)
            {
                CurrentPage = TotalPages;
            }

            // Apply pagination
            Accounts = allAccounts
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }
    }
}
