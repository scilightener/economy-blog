using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using HttpServer.Attributes;

namespace HttpServer;

internal static partial class ServerResponseProvider
{
    public static ServerResponse GetResponse(string path, string rawUrl)
    {
        if (!Directory.Exists(path))
            return new ServerResponse(
                Encoding.UTF8.GetBytes($"Directory {path} not found."),
                "text/plain");

        var buffer = GetFile(path + rawUrl.Replace("%20", " "));
        var contentType = GetContentType(rawUrl);
        if (buffer.Length == 0)
        {
            contentType = "text/plain";
            buffer = Encoding.UTF8.GetBytes("Error 404. Not Found.");
        }
        
        return new ServerResponse(buffer, contentType);
    }
    
    private static bool MethodHandler(HttpListenerContext _httpContext)
        {
            var request = _httpContext.Request;

            var response = _httpContext.Response;

            if (_httpContext.Request.Url.Segments.Length < 2) return false;

            var controllerName = _httpContext.Request.Url.Segments[1].Replace("/", "");

            var strParams = _httpContext.Request.Url
                                    .Segments
                                    .Skip(2)
                                    .Select(s => s.Replace("/", ""))
                                    .ToArray();

            var assembly = Assembly.GetExecutingAssembly();

            var controller = assembly.GetTypes()
                .Where(t => Attribute.IsDefined(t, typeof(HttpController)))
                .FirstOrDefault(c => string.Equals(c.Name, controllerName, StringComparison.CurrentCultureIgnoreCase));

            if (controller == null) return false;

            var test = nameof(HttpController);
            var method = controller.GetMethods()
                .FirstOrDefault(t => t.GetCustomAttributes(true)
                    .Any(attr => attr.GetType().Name == $"Http{_httpContext.Request.HttpMethod}"));
            
            if (method == null) return false;

            var queryParams = method.GetParameters()
                                .Select((p, i) => Convert.ChangeType(strParams[i], p.ParameterType))
                                .ToArray();

            var ret = method.Invoke(Activator.CreateInstance(controller), queryParams);

            response.ContentType = "application/json";

            var buffer = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(ret));
            response.ContentLength64 = buffer.Length;

            var output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);

            output.Close();
            
            return true;
        }

    private static byte[] GetFile(string filePath)
    {
        var buffer = Array.Empty<byte>();

        if (Directory.Exists(filePath))
        {
            filePath += "/index.html";
            if (File.Exists(filePath))
                buffer = File.ReadAllBytes(filePath);
        }
        else if (File.Exists(filePath))
            buffer = File.ReadAllBytes(filePath);

        return buffer;
    }

    private static string GetContentType(string path)
    {
        var ext = path.Contains('.') ? path.Split('.')[^1] : "html";
        return ContentTypeDictionary.ContainsKey(ext) ? ContentTypeDictionary[ext] : "text/plain";
    }
}