using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HotelStay.Api.JsonConverters;

public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string Format = "yyyy-MM-dd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var s = reader.GetString();
        if (string.IsNullOrEmpty(s)) return DateOnly.MinValue;
        if (DateOnly.TryParse(s, out var d)) return d;
        // Try parsing with exact format as fallback
        if (DateOnly.TryParseExact(s, Format, out d)) return d;
        throw new JsonException($"Unable to parse DateOnly from \"{s}\"");
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}
