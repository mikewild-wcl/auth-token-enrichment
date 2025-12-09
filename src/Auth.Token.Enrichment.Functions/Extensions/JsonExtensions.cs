using System.IO;
using System.Text;
using System.Text.Json;

namespace Auth.Token.Enrichment.Functions.Extensions;

internal static class JsonExtensions
{
    public static string FormatJson(this string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return string.Empty;
        }

        var jsonDocument = JsonDocument.Parse(json);

        return jsonDocument.FormatJson();
    }

    public static string FormatJson(this JsonDocument jsonDocument)
    {
        if (jsonDocument is null)
        {
            return string.Empty;
        }

        var options = new JsonWriterOptions
        {
            Indented = true
        };

        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream, options))
        {
            jsonDocument.WriteTo(writer);
        }

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    public static string? SafeGetString(
        this JsonElement element,
        string propertyName,
        int maxLength = -1,
        string? defaultValue = default)
    {
        var result = element.ValueKind != JsonValueKind.Undefined
                     && element.TryGetProperty(propertyName, out var property)
                     && property.ValueKind == JsonValueKind.String
            ? property.GetString()
            : defaultValue;

        return result is not null && maxLength > 0 && result.Length > maxLength
            ? result[..maxLength].Trim()
            : result;
    }
}
