using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace GammaRaySignaling;

public class Common
{
    public static string GetCurrentTime()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
    }

    public static long GetCurrentTimestamp()
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
    }
    
    public static string Md5String(string input)
    {
        using MD5 md5 = MD5.Create();
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);
        var sb = new StringBuilder();
        foreach (var t in hashBytes)
        {
            sb.Append(t.ToString("x2"));
        }
        return sb.ToString();
    }

    public static string MakeJsonMessage(int code, string msg, Dictionary<string, string> value)
    {
        var resp = new Dictionary<string, object>
        {
            {"code", code},
            {"msg", msg},
            {"value", value}
        };
        return JsonSerializer.Serialize(resp);
    }

    public static string MakeOkJsonMessage(Dictionary<string, string> value)
    {
        return MakeJsonMessage(200, "Ok", value);
    }
}