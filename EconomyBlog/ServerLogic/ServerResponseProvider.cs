using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using EconomyBlog.Services;

namespace EconomyBlog.ServerLogic;

internal static class ServerResponseProvider
{
    public static HttpListenerResponse GetResponse(string path, HttpListenerContext ctx)
    {
        var request = ctx.Request;
        var response = ctx.Response;
        if (TryHandleController(request, response))
            return response;
        HandleActionResult(response, new ErrorResult(FileOrDirectoryNotFound));
        return response;
    }

    private static bool TryHandleController(HttpListenerRequest request, HttpListenerResponse response)
    {
        if (request.Url!.Segments.Length < 2) return false;
        if (!request.Url.Segments[^1].Contains('.') && !request.RawUrl!.EndsWith('/'))
        {
            response.Redirect(request.Url + "/");
            return true;
        }

        using var sr = new StreamReader(request.InputStream, request.ContentEncoding);
        var bodyParam = sr.ReadToEnd();

        var path = string.Join("", request.Url.Segments.Skip(2));
        var strParams = request.HttpMethod == "POST"
            ? bodyParam.Split('&').Select(p => p.Split('=').LastOrDefault()).Select(HttpUtility.UrlDecode).ToArray()
            : new[] { path };

        var controllerName = request.Url.Segments[1].Replace("/", "");
        var assembly = Assembly.GetExecutingAssembly();
        var controller = assembly.GetTypes()
            .Where(t => Attribute.IsDefined(t, typeof(HttpController)))
            .FirstOrDefault(t => string.Equals(
                (t.GetCustomAttribute(typeof(HttpController)) as HttpController)?.ControllerName,
                controllerName,
                StringComparison.CurrentCultureIgnoreCase));

        var methodString = string.Concat(request.Url.Segments.Skip(2));
        var method = controller?.GetMethods()
            .FirstOrDefault(t => t.GetCustomAttributes(true)
                .Any(attr => attr.GetType().Name == $"Http{request.HttpMethod}"
                             && Regex.IsMatch(methodString,
                                 attr.GetType()
                                     .GetField("MethodUri")?
                                     .GetValue(attr)?.ToString() ?? "")));

        var queryParams = new object[] { GetSessionGuid(request) }.Concat(
                method?.GetParameters()
                    .Skip(1)
                    .Select((p, i) => i < strParams.Length
                        ? Convert.ChangeType(strParams[i], p.ParameterType)
                        : p.DefaultValue) ?? Array.Empty<object?>())
            .ToArray();

        try
        {
            if (method?.Invoke(Activator.CreateInstance(controller!), queryParams) is not ActionResult ret)
                return false;
            HandleActionResult(response, ret);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            HandleActionResult(response, new ErrorResult(ServerFault));
        }

        return true;
    }

    private static Guid GetSessionGuid(HttpListenerRequest request)
    {
        var cookie = request.Cookies["SessionId"]?.Value ?? "";
        if (!Guid.TryParse(cookie, out var parsed)) return Guid.Empty;
        return SessionManager.CheckSession(parsed)
            ? parsed
            : Guid.Empty;
    }

    private static void HandleActionResult(HttpListenerResponse response, ActionResult result)
    {
        if (result.StatusCode is HttpStatusCode.Redirect)
            response.Redirect(result.RedirectUrl!);
        response.StatusCode = (int)result.StatusCode;
        response.Headers.Set("Content-Type", result.ContentType);
        if (result.Cookies != null) response.Cookies.Add(result.Cookies);
        response.OutputStream.Write(result.Buffer);
    }
}