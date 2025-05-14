using Microsoft.Extensions.FileProviders;
using Microsoft.EntityFrameworkCore;
using Nexttech.Data;

var builder = WebApplication.CreateBuilder(args);

// Set up database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql(connectionString, ServerVersion.Create(new Version(8, 0, 0), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql)));

// Add CORS
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", builder => 
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddControllers();

var app = builder.Build();


//middleware
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

// Map controller routes
app.MapControllers();

// Serve default file and static files
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

//test route
app.MapGet("/api/hello", () => "Hello from the Web API!");

//custom port
app.Urls.Add("http://localhost:5077");

app.Run();
