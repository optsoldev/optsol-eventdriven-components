using System.Text.Json;
using System.Text.Json.Serialization;

namespace System;

public static class GenericExtensions
{
    public static string ToJson<T>(this T source)
    {
        if (source is null)
            return "{}";

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        return JsonSerializer.Serialize(source, options);
    }
}