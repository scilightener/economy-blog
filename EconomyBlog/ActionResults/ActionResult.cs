using System.Net;

namespace EconomyBlog.ActionResults;

public abstract class ActionResult
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public string ContentType { get; protected set; } = "text/html";
    public byte[] Buffer { get; protected set; } = Array.Empty<byte>();
    public string? RedirectUrl { get; protected set; }
    public CookieCollection? Cookies { get; protected set; }

}