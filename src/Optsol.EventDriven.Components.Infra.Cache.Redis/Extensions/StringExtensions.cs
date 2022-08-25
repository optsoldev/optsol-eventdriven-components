using System;
using System.Text;
using System.Text.Json;

namespace System;

public static class StringExtensions
{
    public static T? To<T>(this string source)
    {
        if (source.IsNullOrWhiteSpace())
            return default;

        return JsonSerializer.Deserialize<T>(source);
    }

    public static byte[] ToBytes(this string source)
        => Encoding.UTF8.GetBytes(source);

    public static bool IsNotNullOrWhiteSpace(this string? source)
        => !string.IsNullOrWhiteSpace(source);

    public static bool IsNullOrWhiteSpace(this string? source)
        => string.IsNullOrWhiteSpace(source);
}