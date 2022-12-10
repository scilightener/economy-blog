using System.Security.Cryptography;
using System.Text;

namespace EconomyBlog;

internal static class Hashing
{
    public static string Hash(string value)
    {
        using var md5 = MD5.Create();
        return Convert.ToHexString(md5.ComputeHash(Encoding.UTF8.GetBytes(value)));
    }
}