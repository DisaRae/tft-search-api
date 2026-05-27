using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TFT.Search.Library
{
    public class NullToZeroJsonConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // If the token is null, return 0 instead of throwing an exception
            if (reader.TokenType == JsonTokenType.Null) return 0;
            return reader.GetInt32();
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
