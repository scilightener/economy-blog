using System.Net;
using Scriban;
using static EconomyBlog.Messages;

namespace EconomyBlog.ActionResults;

public class ActionResult
{
    public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;
    public string ContentType { get; init; } = "text/html";
    public byte[] Buffer { get; init; } = Array.Empty<byte>();
    public string? RedirectUrl { get; init; }
    public CookieCollection? Cookies { get; init; }

    internal static string GetHtml(string path, object? model)
    {
        if (!File.Exists(path)) return FileOrDirectoryNotFound;
        var template = File.ReadAllText(path);
        var parsed = Template.Parse(template);
        return parsed.Render(model);
    }
}