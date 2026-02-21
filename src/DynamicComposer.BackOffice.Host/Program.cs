using DynamicComposer.Persistence.EF;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDynamicComposerPersistence(builder.Configuration.GetConnectionString("Default") ?? "Data Source=dynamic-composer.db");
builder.Services.AddSpaStaticFiles(options => options.RootPath = "ClientApp/dist");

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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("FrontOffice");
app.UseStaticFiles();
app.UseSpaStaticFiles();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
