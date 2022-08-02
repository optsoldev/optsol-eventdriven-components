﻿using System.Text;

public static class StringExtension
{
    public static string ToKebabCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        var builder = new StringBuilder();
        builder.Append(char.ToLower(str.First()));

        foreach (var c in str.Skip(1))
        {
            if (char.IsUpper(c))
            {
                builder.Append('-');
                builder.Append(char.ToLower(c));
            }
            else
            {
                builder.Append(c);
            }
        }

        return builder.ToString();
    }
}