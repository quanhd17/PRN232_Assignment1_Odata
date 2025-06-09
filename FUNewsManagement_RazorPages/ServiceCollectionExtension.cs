using Microsoft.AspNetCore.Authentication.Cookies;

namespace FUNewsManagement_RazorPages
{
    public static class ServiceCollectionExtension
    {
        public static void AddAuthen(this IServiceCollection services)
        {
            services.AddAuthentication("MyCookieAuth")
                .AddCookie("MyCookieAuth", options =>
                {
                    options.Cookie.Name = "MyAuthCookie";
                    options.LoginPath = "/SystemAccounts/Login";
                    options.AccessDeniedPath = "/Error";
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("StaffOnly", policy => policy.RequireRole("Staff"));
                options.AddPolicy("LecturerOnly", policy => policy.RequireRole("Lecturer"));
            });
        }
    }
}
