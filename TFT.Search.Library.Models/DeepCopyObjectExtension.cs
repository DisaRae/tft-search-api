using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TFT.Search.Library.Models
{
    public static class DeepCopyObjectExtension
    {
        public static T DeepCopy<T>(this T original)
        {
            // Serialize the object to a JSON string
            string json = JsonSerializer.Serialize(original);

            // Deserialize the JSON string back into a new object
            return JsonSerializer.Deserialize<T>(json);
        }

    }
}
