using Microsoft.EntityFrameworkCore;
using DataAccess;
using Assignment1_NgoDongquan.Configuration;
using Microsoft.AspNetCore.OData;
using Assignment1_NgoDongquan.Mapper;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using BusinessObject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    })
    .AddOData(opt =>
    {
        opt.Select().Filter().Expand().OrderBy().Count().SetMaxTop(100)
            .AddRouteComponents("odata", GetEdmModel());
    });
static IEdmModel GetEdmModel()
{
    var edmBuilder = new ODataConventionModelBuilder();
    edmBuilder.EntitySet<NewsArticle>("NewsArticles");
    edmBuilder.EntityType<NewsArticle>()
    .Collection
    .Function("GetActiveNews")
    .ReturnsCollectionFromEntitySet<NewsArticle>("NewsArticle");
    edmBuilder.EntitySet<Category>("Categories");
    edmBuilder.EntityType<Category>()
    .Collection
    .Function("Active")
    .ReturnsCollectionFromEntitySet<Category>("Categories");
    edmBuilder.EntitySet<Tag>("Tags");
    edmBuilder.EntitySet<Comment>("Comments");
    edmBuilder.EntityType<Comment>()
    .Collection
    .Function("GetByArticleId")
    .ReturnsCollectionFromEntitySet<Comment>("Comments")
    .Parameter<int>("articleId");
    edmBuilder.EntityType<SystemAccount>()
        .Collection
        .Function("Any")
        .ReturnsFromEntitySet<SystemAccount>("SystemAccounts");
    return edmBuilder.GetEdmModel();
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FunewsManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCustomServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:5001", "https://localhost:7209", "http://localhost:7209")
              .AllowCredentials()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
    
    // More permissive policy for development
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddAutoMapper(typeof(AutoMappingProfile));

builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.Cookie.Name = "MyAuthCookie";
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

// Use more permissive CORS policy for development
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAll");
}
else
{
    app.UseCors("AllowFrontend");
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
