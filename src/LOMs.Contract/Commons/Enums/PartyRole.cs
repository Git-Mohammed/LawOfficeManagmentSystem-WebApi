using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LOMs.Contract.Commons.Enums;

[TypeConverter(typeof(cPartyRoleJsonConverter))]
[JsonConverter(typeof(PartyRoleArabicConverter))]
public enum PartyRole
{
    [Display(Name = "مدعي")]
    Plaintiff = 1,

    [Display(Name = "مدعى عليه")]
    Defendant = 2
}

public class cPartyRoleJsonConverter : JsonConverter<PartyRole>
{

    // JsonConverter for JSON binding
    public override PartyRole Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "مدعي" => PartyRole.Plaintiff,
            "مدعي عليه" => PartyRole.Defendant,
            _ => throw new JsonException($"قيمة غير صالحة: {value}")
        };
    }

    public override void Write(Utf8JsonWriter writer, PartyRole value, JsonSerializerOptions options)
    {
        var stringValue = value switch
        {
            PartyRole.Plaintiff => "مدعي",
            PartyRole.Defendant => "مدعى عليه",
            _ => throw new JsonException($"قيمة غير معروفة: {value}")
        };

        writer.WriteStringValue(stringValue);
    }
}

public class PartyRoleArabicConverter : TypeConverter
{
    // TypeConverter for query string binding
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        var stringValue = value?.ToString()?.Trim();
        return stringValue switch
        {
            "مدعي" => PartyRole.Plaintiff,
            "مدعى عليه" => PartyRole.Defendant,
            _ => throw new ArgumentException($"قيمة دور الطرف غير معروفة: {stringValue}")
        };
    }

}
