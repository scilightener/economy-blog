namespace EconomyBlog.Structures;

internal struct ContentTypeProvider
{
    private static readonly Dictionary<string, string> Allowed = new()
    {
        {"bmp", "image/bmp"},
        {"gif", "image/gif"},
        {"jpeg", "image/jpeg"},
        {"jpg", "image/jpeg"},
        {"png", "image/png"},
        {"svg", "image/svg+xml"},
        {"webp", "image/webp"},
        
        {"avi", "video/x-msvideo"},
        {"mkv", "video/x-matroska"},
        {"mp4", "video/mp4"},
        {"mpeg", "video/mpeg"},
        {"mpv", "video/mpv"},
        {"webm", "video/webm"},
        
        {"aac", "audio/mp4"},
        {"mp3", "audio/mpeg"},
        {"ogg", "audio/ogg"},
        
        {"css", "text/css"},
        {"csv", "text/csv"},
        {"html", "text/html"},
        {"php", "text/php"},
        {"txt", "text/plain"},

        {"doc", "application/msword"},
        {"docx", "application/msword"},
        {"js", "application/javascript"},
        {"json", "application/json"},
        {"pdf", "application/pdf"},
        {"tex", "application/x-tex"},
        {"xml", "application/xml"},
    };
    
    public static string GetContentType(string path)
    {
        var ext = path.Contains('.') ? path.Split('.')[^1] : "html";
        return Allowed.ContainsKey(ext) ? Allowed[ext] : "text/plain";
    }
}