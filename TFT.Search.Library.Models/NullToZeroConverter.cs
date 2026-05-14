using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFT.Search.Library
{
    public class NullToZeroConverter : JsonConverter<int>
    {
        public override void WriteJson(JsonWriter writer, int value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }

        public override int ReadJson(JsonReader reader, Type objectType, int existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // If the token is null, return 0 instead of throwing an exception
            if (reader.TokenType == JsonToken.Null) return 0;

            return Convert.ToInt32(reader.Value);
        }
    }
}
