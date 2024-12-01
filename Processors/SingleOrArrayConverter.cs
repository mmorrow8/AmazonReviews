using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AmazonReviews.Processors
{
    public class SingleOrArrayConverter<T> : JsonConverter<List<T>>
    {
        public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                // Deserialize as a list
                return JsonSerializer.Deserialize<List<T>>(ref reader, options);
            }
            else
            {
                // Deserialize as a single item and wrap it in a list
                var singleValue = JsonSerializer.Deserialize<T>(ref reader, options);
                return new List<T> { singleValue };
            }
        }

        public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
        {
            if (value.Count == 1)
            {
                // Serialize as a single value
                JsonSerializer.Serialize(writer, value[0], options);
            }
            else
            {
                // Serialize as an array
                JsonSerializer.Serialize(writer, value, options);
            }
        }
    }
}