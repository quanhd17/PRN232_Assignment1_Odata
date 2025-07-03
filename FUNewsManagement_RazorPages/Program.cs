using FUNewsManagement_RazorPages;
using FUNewsManagement_RazorPages.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient();
builder.Services.AddAuthen();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    })
    .AddOData(opt =>
    {
        opt.Select().Filter().Expand().OrderBy().Count().SetMaxTop(100)
            .AddRouteComponents("odata", GetEdmModel());
    });
static IEdmModel GetEdmModel()
{
    var edmBuilder = new ODataConventionModelBuilder();
    edmBuilder.EntityType<NewsArticle>()
     .Collection
     .Function("GetActiveNews")
     .ReturnsCollectionFromEntitySet<NewsArticle>("NewsArticle");
    edmBuilder.EntitySet<Category>("Categories");
    edmBuilder.EntitySet<Tag>("Tags");
    edmBuilder.EntityType<SystemAccount>()
        .Collection
        .Function("Any")
        .ReturnsFromEntitySet<SystemAccount>("SystemAccounts");
    return edmBuilder.GetEdmModel();
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
