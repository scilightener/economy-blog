using System.Net;

namespace EconomyBlog.ActionResults;

public class ActionResult
{
    public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;
    public string ContentType { get; init; } = "text/html";
    public byte[] Buffer { get; init; } = Array.Empty<byte>();
    public string? RedirectUrl { get; init; }
    public CookieCollection? Cookies { get; init; }
}