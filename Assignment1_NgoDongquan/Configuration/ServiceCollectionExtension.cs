using BusinessLogic.Service;
using BusinessLogic.UnitOfWork;
using DataAccess.Repositories;

namespace Assignment1_NgoDongquan.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            // Register your services here
            // Example: services.AddScoped<IMyService, MyService>();

            // Add other necessary service registrations

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<ITagService, TagService>();
            services.AddScoped<INewsArticleService, NewsArticleService>();
            services.AddScoped<ISystemAccountService, SystemAccountService>();
            services.AddScoped<ICommentService, CommentService>();
        }
    }
}
