// --- WebApi/Common/Caching/ETagGenerator.cs ---
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace WebApi.Common.Caching
{
    public static class ETagGenerator
    {
        // Recibe cualquier objeto, lo hace JSON y genera el ETag
        public static string Generate<T>(T content)
        {
            var jsonString = JsonSerializer.Serialize(content);
            byte[] hashBytes = MD5.HashData(Encoding.UTF8.GetBytes(jsonString));
            return $"\"{Convert.ToHexString(hashBytes)}\"";
        }
    }
}