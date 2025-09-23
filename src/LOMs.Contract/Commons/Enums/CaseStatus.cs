using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LOMs.Contract.Commons.Enums;

[TypeConverter(typeof(CaseStatusArabicJsonConverter))]
[JsonConverter(typeof(CaseStatusArabicTypeConverter))]
public enum CaseStatus
{
    [Display(Name = "مسودة")]
    Draft = 0,

    [Display(Name = "معلق")]
    Pending = 1,

    [Display(Name = "قيد المعالجة")]
    Processing = 2,

    [Display(Name = "مكتمل")]
    Done = 3,

    [Display(Name = "ملغي")]
    Cancelled = 4
}

public class CaseStatusArabicJsonConverter : JsonConverter<CaseStatus>
{
    public override CaseStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "مسودة" => CaseStatus.Draft,
            "معلق" => CaseStatus.Pending,
            "قيد المعالجة" => CaseStatus.Processing,
            "مكتمل" => CaseStatus.Done,
            "ملغي" => CaseStatus.Cancelled,
            _ => throw new JsonException($"قيمة غير صالحة: {reader.GetString()}")
        };
    }

    public override void Write(Utf8JsonWriter writer, CaseStatus value, JsonSerializerOptions options)
    {
        var stringValue = value switch
        {
            CaseStatus.Draft => "مسودة",
            CaseStatus.Pending => "معلق",
            CaseStatus.Processing => "قيد المعالجة",
            CaseStatus.Done => "مكتمل",
            CaseStatus.Cancelled => "ملغي",
            _ => throw new JsonException($"قيمة غير معروفة: {value}")
        };

        writer.WriteStringValue(stringValue);
    }
}
public class CaseStatusArabicTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return value?.ToString()?.Trim() switch
        {
            "مسودة" => CaseStatus.Draft,
            "معلق" => CaseStatus.Pending,
            "قيد المعالجة" => CaseStatus.Processing,
            "مكتمل" => CaseStatus.Done,
            "ملغي" => CaseStatus.Cancelled,
            _ => throw new ArgumentException($"قيمة الحالة غير معروفة: {value}")
        };
    }
}


