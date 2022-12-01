using System.Net;

namespace EconomyBlog.ActionResults;

public class ActionResult
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public string ContentType { get; set; } = "text/html";
    public byte[] Buffer { get; set; } = Array.Empty<byte>();
    public string? RedirectUrl { get; set; }
    public CookieCollection? Cookies { get; set; }
}