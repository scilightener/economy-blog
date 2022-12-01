using System.Net;

namespace EconomyBlog.ActionResults;

public class ActionResult
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public string ContentType { get; set; } = "text/html";
    public byte[] Buffer { get; init; } = Array.Empty<byte>();
    public string? RedirectUrl { get; set; }
    public CookieCollection? Cookies { get; set; }
}