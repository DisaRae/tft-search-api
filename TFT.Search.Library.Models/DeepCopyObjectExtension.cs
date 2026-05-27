using System.Text.Json;

namespace TFT.Search.Library.Models
{
    public static class DeepCopyObjectExtension
    {
        public static T DeepCopy<T>(this T original)
        {
            string json = JsonSerializer.Serialize(original);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
