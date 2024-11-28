
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy, as the UI is in a different endpoint
builder.Services.AddCors(options =>
{
    options.AddPolicy("PTTTechTest", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

ConfigSerilog(builder);

builder.Services.AddDbContext<ImageDbContext>(options => options.UseSqlite("DataSource=:data.db"));

builder.Services.AddScoped<RouteMapper>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IImageFetcherService, ImageFetcherService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    SeedDatabase(scope);

    var routeMapper = scope.ServiceProvider.GetRequiredService<RouteMapper>();
    routeMapper.MapRoutes(app);

}

app.UseCors("PTTTechTest");

app.UseHttpsRedirection();

app.Run();



static void ConfigSerilog(WebApplicationBuilder builder)
{
    builder.Host.UseSerilog((fileContext, loggingConfig) =>
    {
        loggingConfig.ReadFrom.Configuration(fileContext.Configuration);
    });
}

static void SeedDatabase(IServiceScope scope)
{
    var context = scope.ServiceProvider.GetRequiredService<ImageDbContext>();

    context.Database.OpenConnection();
    context.Database.EnsureCreated();


    // Apply migrations
    context.Database.Migrate();

    //Preventing repetitive seeding
    if (context != null && context.Images != null && !context.Images.Any())
    {
        context.Images.Add(new Image { Id = 1, Url = "https://api.dicebear.com/9.x/avataaars/svg?seed=Luis&size=150" });
        context.Images.Add(new Image { Id = 2, Url = "https://api.dicebear.com/9.x/avataaars/svg?seed=John&size=150" });
        context.Images.Add(new Image { Id = 3, Url = "https://api.dicebear.com/9.x/avataaars/svg?seed=Jane&size=150" });
        context.Images.Add(new Image { Id = 4, Url = "https://api.dicebear.com/9.x/avataaars/svg?seed=Paul&size=150" });
        context.Images.Add(new Image { Id = 5, Url = "https://api.dicebear.com/9.x/avataaars/svg?seed=Mark&size=150" });

        context.SaveChanges();
    }

}