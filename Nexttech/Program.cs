using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Serve static files from the "wwwroot" directory
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "html/index.html" } // Default page
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), // Path to wwwroot folder
    RequestPath = "" // Serve files at the root URL
});

// Example API endpoint (optional)
app.MapGet("/api/hello", () => "Hello from the Web API!");

// Run the app on http://localhost:8080
app.Urls.Add("http://localhost:5077");
app.Run();
