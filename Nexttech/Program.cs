using System.Net;
using System.IO;

HttpListener server = new HttpListener();
server.Prefixes.Add("http://localhost:8080/"); // Listening on port 8080
server.Start();

Console.WriteLine("Web server running at http://localhost:8080/");
Console.WriteLine("Press Ctrl+C to stop.");

while (true)
{
    var context = server.GetContext(); // Wait for a request
    var request = context.Request;
    var response = context.Response;

    // Get the requested URL path
    string urlPath = request.Url!.AbsolutePath.Trim('/');

    // If no path is specified, serve the default 'index.html'
    if (string.IsNullOrEmpty(urlPath)) 
        urlPath = "html/index.html"; // Default to the index.html inside 'html' folder
    
    // Build the file path based on the requested URL
    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "public", urlPath);

    if (File.Exists(filePath))
    {
        byte[] content = File.ReadAllBytes(filePath);
        response.ContentType = GetContentType(filePath);  // Set correct content type (html, css, js)
        response.ContentLength64 = content.Length;
        response.OutputStream.Write(content, 0, content.Length);
    }
    else
    {
        // If file is not found, return a 404 page
        string error = "<h1>404 - Page not found</h1>";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(error);
        response.StatusCode = 404;
        response.ContentType = "text/html";
        response.OutputStream.Write(buffer, 0, buffer.Length);
    }

    response.OutputStream.Close();
}

// Helper method to return content type based on file extension
static string GetContentType(string filePath)
{
    return Path.GetExtension(filePath) switch
    {
        ".html" => "text/html",
        ".css" => "text/css",
        ".js" => "application/javascript",
        ".png" => "image/png",
        ".jpg" => "image/jpeg",
        _ => "application/octet-stream",
    };
}
