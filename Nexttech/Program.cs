using Microsoft.Extensions.FileProviders;
using Microsoft.EntityFrameworkCore;
using Nexttech.Data;


var builder = WebApplication.CreateBuilder(args);

// Connection string from appsettings.json or direct inline (if needed)
var connectionString = "server=localhost;database=printdb;user=root;password=;";

// This is from Pomelo
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql(connectionString, ServerVersion.Create(new Version(8, 0, 0), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql)));

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "html/index.html" }
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = ""
});

app.MapGet("/api/hello", () => "Hello from the Web API!");

app.Urls.Add("http://localhost:5077");

app.Run();
