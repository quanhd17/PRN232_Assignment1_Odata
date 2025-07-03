using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace FUNewsManagement_RazorPages.Pages.Reports
{
    [Authorize(Roles = "Admin")]
    public class ReportsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public ReportsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public int TotalArticles { get; set; }
        public int PublishedArticles { get; set; }
        public int TotalCategories { get; set; }
        public int ParentCategories { get; set; }
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalComments { get; set; }

        // User Statistics by Role
        public int AdminUsers { get; set; }
        public int StaffUsers { get; set; }
        public int LecturerUsers { get; set; }
        public int InactiveUsers { get; set; }

        public List<CategoryStat> CategoryStats { get; set; } = new List<CategoryStat>();

        public List<ActivityItem> RecentActivities { get; set; } = new List<ActivityItem>();

        public async Task OnGetAsync()
        {
            try
            {
                await LoadStatistics();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading statistics: {ex.Message}");
                // Set default values if API fails
                SetDefaultValues();
            }
        }

        private async Task LoadStatistics()
        {
            var articlesResponse = await _httpClient.GetFromJsonAsync<ODataResponse<NewsArticle>>("https://localhost:7130/odata/NewsArticles");
            var articles = articlesResponse?.Value ?? new List<NewsArticle>();
            
            TotalArticles = articles.Count;
            PublishedArticles = articles.Count(a => a.NewsStatus == true);

            var categoriesResponse = await _httpClient.GetFromJsonAsync<ODataResponse<Category>>("https://localhost:7130/odata/Categories");
            var categories = categoriesResponse?.Value ?? new List<Category>();
            
            TotalCategories = categories.Count;
            ParentCategories = categories.Count(c => !c.ParentCategoryId.HasValue);

            var usersResponse = await _httpClient.GetFromJsonAsync<ODataResponse<SystemAccount>>("https://localhost:7130/odata/SystemAccounts");
            var users = usersResponse?.Value ?? new List<SystemAccount>();
            
            TotalUsers = users.Count;
            ActiveUsers = users.Count(u => u.IsActive);
            
            AdminUsers = users.Count(u => u.AccountRole == 0); 
            StaffUsers = users.Count(u => u.AccountRole == 1); 
            LecturerUsers = users.Count(u => u.AccountRole == 2);
            InactiveUsers = users.Count(u => !u.IsActive);

            // Load comments
            var commentsResponse = await _httpClient.GetFromJsonAsync<ODataResponse<Comment>>("https://localhost:7130/odata/Comments");
            var comments = commentsResponse?.Value ?? new List<Comment>();
            
            // Count comments from last 30 days
            var thirtyDaysAgo = DateTime.Now.AddDays(-30);
            TotalComments = comments.Count(c => c.CreatedDate >= thirtyDaysAgo);

            BuildCategoryStats(articles, categories);

            BuildRecentActivities(articles, users, comments);
        }

        private void BuildCategoryStats(List<NewsArticle> articles, List<Category> categories)
        {
            CategoryStats = categories.Select(category => new CategoryStat
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                ArticleCount = articles.Count(a => a.CategoryId == category.CategoryId)
            }).OrderByDescending(c => c.ArticleCount).ToList();
        }

        private void BuildRecentActivities(List<NewsArticle> articles, List<SystemAccount> users, List<Comment> comments)
        {
            var activities = new List<ActivityItem>();

            // Add recent article activities
            var recentArticles = articles.OrderByDescending(a => a.CreatedDate).Take(5);
            foreach (var article in recentArticles)
            {
                activities.Add(new ActivityItem
                {
                    Title = $"New Article: {article.NewsTitle}",
                    Description = $"Article created by {GetUserName(article.CreatedById, users)}",
                    Date = article.CreatedDate ?? DateTime.Now
                });
            }

            // Add recent comment activities
            var recentComments = comments.OrderByDescending(c => c.CreatedDate).Take(3);
            foreach (var comment in recentComments)
            {
                activities.Add(new ActivityItem
                {
                    Title = "New Comment Added",
                    Description = $"Comment by {GetUserName(comment.AccountId, users)} on article",
                    Date = comment.CreatedDate ?? DateTime.Now
                });
            }

            // Add user registration activities
            var recentUsers = users.OrderByDescending(u => u.AccountId).Take(2);
            foreach (var user in recentUsers)
            {
                activities.Add(new ActivityItem
                {
                    Title = $"New User: {user.AccountName}",
                    Description = $"User registered with role {GetRoleName(user.AccountRole)}",
                    Date = DateTime.Now.AddDays(-new Random().Next(1, 10))
                });
            }

            RecentActivities = activities.OrderByDescending(a => a.Date).Take(8).ToList();
        }

        private string GetUserName(short? userId, List<SystemAccount> users)
        {
            if (!userId.HasValue) return "Unknown";
            var user = users.FirstOrDefault(u => u.AccountId == userId);
            return user?.AccountName ?? "Unknown";
        }

        private string GetRoleName(int role)
        {
            return role switch
            {
                0 => "Administrator",
                1 => "Staff",
                2 => "Lecturer",
                _ => "Unknown"
            };
        }

        private void SetDefaultValues()
        {
            TotalArticles = 0;
            PublishedArticles = 0;
            TotalCategories = 0;
            ParentCategories = 0;
            TotalUsers = 0;
            ActiveUsers = 0;
            TotalComments = 0;
            AdminUsers = 0;
            StaffUsers = 0;
            LecturerUsers = 0;
            InactiveUsers = 0;
            CategoryStats = new List<CategoryStat>();
            RecentActivities = new List<ActivityItem>();
        }
    }

    public class CategoryStat
    {
        public short CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int ArticleCount { get; set; }
    }

    public class ActivityItem
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
} 