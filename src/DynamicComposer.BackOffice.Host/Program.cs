using DynamicComposer.Persistence.EF;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "ClientApp/dist"
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Use Microsoft's OpenAPI support instead of Swashbuckle
builder.Services.AddOpenApi();

builder.Services.AddDynamicComposerPersistence(builder.Configuration.GetConnectionString("Default") ?? "Data Source=dynamic-composer.db");

// Note: AddSpaStaticFiles / UseSpaStaticFiles (SpaServices) are optional/deprecated in newer ASP.NET Core versions.
// Serving the SPA via the web root + static files middleware is the recommended approach.

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontOffice", policy =>
        policy.WithOrigins("https://localhost:7060", "http://localhost:5060")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DynamicComposerDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    // Use Microsoft OpenAPI middleware and UI
    //app.UseOpenApi();
    //app.UseOpenApiUI();
}

app.UseHttpsRedirection();
app.UseCors("FrontOffice");

// Serve static files from the configured web root ("ClientApp/dist")
app.UseStaticFiles();

app.MapControllers();

// Fallback to the SPA entry point (index.html) in the web root
app.MapFallbackToFile("index.html");

app.Run();
