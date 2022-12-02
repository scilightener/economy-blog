using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;
using EconomyBlog.Models;
using EconomyBlog.ServerLogic.SessionLogic;

namespace EconomyBlog.ServerLogic;

internal static class ServerResponseProvider
{
    public static HttpListenerResponse GetResponse(string path, HttpListenerContext ctx)
    {
        var request = ctx.Request;
        var response = ctx.Response;
        var buffer = Encoding.UTF8.GetBytes("Error 404. Not Found.");
        if (!Directory.Exists(path))
            buffer = Encoding.UTF8.GetBytes($"Directory {path} not found.");
        else if (TryHandleController(request, response))
            return response;
        response.OutputStream.Write(buffer);
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
            ? bodyParam.Split('&').Select(p => p.Split('=').LastOrDefault()).ToArray()
            : new[] { path };
        
        var controllerName = request.Url.Segments[1].Replace("/", "");
        var assembly = Assembly.GetExecutingAssembly();
        var controller = assembly.GetTypes()
            .Where(t => Attribute.IsDefined(t, typeof(HttpController)))
            .FirstOrDefault(t => string.Equals(
                (t.GetCustomAttribute(typeof(HttpController)) as HttpController)?.ControllerName,
                controllerName,
                StringComparison.CurrentCultureIgnoreCase));

        var method = controller?.GetMethods()
            .FirstOrDefault(t => t.GetCustomAttributes(true)
                .Any(attr => attr.GetType().Name == $"Http{request.HttpMethod}"
                    && Regex.IsMatch(request.RawUrl ?? "",
                        attr.GetType()
                        .GetField("MethodUri")?
                        .GetValue(attr)?.ToString() ?? "")));

        var queryParams = method?.GetParameters()
            .Select((p, i) => Convert.ChangeType(strParams[i], p.ParameterType))
            .ToArray();

        if (method?.Invoke(Activator.CreateInstance(controller!), queryParams) is not ActionResult ret) return false;
        
        // if (!HandleCookies(request, response, method.Name, ret))
        //     return true;
        
        HandleActionResult(response, ret);
        return true;
    }

    // private static bool HandleCookies(HttpListenerRequest request, HttpListenerResponse response, string methodName, object? ret)
    // {
    //     var sessionCookie = request.Cookies["SessionId"]?.Value ?? "";
    //     switch (methodName)
    //     {
    //         case "Login":
    //             var retParsed = ((int, Guid))(ret ?? (-1, Guid.Empty));
    //             if (retParsed == (-1, Guid.Empty))
    //                 return true;
    //             response.Cookies.Add(new Cookie("SessionId", retParsed.Item2.ToString()));
    //             Console.WriteLine();
    //             return false;
    //         case "GetAccounts":
    //             if (SessionManager.CheckSession(Guid.Parse(sessionCookie)))
    //                 return true;
    //             FillResponse(response, "text/plain", (int)HttpStatusCode.Unauthorized, Array.Empty<byte>());
    //             return false;
    //         case "GetAccountInfo":
    //             var currentSession = SessionManager.GetSessionInfo(Guid.Parse(sessionCookie));
    //             if (ret is not null && currentSession is not null && currentSession.AccountId == ((User)ret).Id)
    //                 return true;
    //             FillResponse(response, "text/plain", (int)HttpStatusCode.Unauthorized, Array.Empty<byte>());
    //             return false;
    //         default:
    //             return true;
    //     }
    // }
    

    private static void HandleActionResult(HttpListenerResponse response, ActionResult result)
    {
        response.Headers.Set("Content-Type", result.ContentType);
        response.StatusCode = (int)result.StatusCode;
        response.OutputStream.Write(result.Buffer);
        if (result.StatusCode is HttpStatusCode.Redirect)
            response.Redirect(result.RedirectUrl!);
    }
}