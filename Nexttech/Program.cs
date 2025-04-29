using System.Net;

HttpListener server = new HttpListener();
server.Prefixes.Add("http://localhost:8080/"); // Listening on port 8080
server.Start();

Console.WriteLine("Web server running at http://localhost:8080/");
Console.WriteLine("Press Ctrl+C to stop.");

while (true)
{
    var context = server.GetContext(); // Wait for a request
    var response = context.Response;

    // Serve the index.html file by default
    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "public", "index.html");

    if (File.Exists(filePath))
    {
        byte[] content = File.ReadAllBytes(filePath);
        response.ContentType = "text/html";
        response.ContentLength64 = content.Length;
        response.OutputStream.Write(content, 0, content.Length);
    }
    else
    {
        string error = "<h1>404 - Page not found</h1>";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(error);
        response.StatusCode = 404;
        response.ContentType = "text/html";
        response.OutputStream.Write(buffer, 0, buffer.Length);
    }

    response.OutputStream.Close();
}
