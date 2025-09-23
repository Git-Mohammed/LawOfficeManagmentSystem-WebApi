using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LOMs.Domain.Cases.Enums
{
    [JsonConverter(typeof(CaseStatusJsonConverter))]
    [TypeConverter(typeof(CaseStatusTypeConverter))]
    public enum CaseStatus
    {
        Draft = 0,       // مسودة
        Pending = 1,     // معلق
        Processing = 2,  // قيد المعالجة
        Complete = 3,    // مكتمل
        Cancelled = 4    // ملغي
    }

    public class CaseStatusTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            => sourceType == typeof(string);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            var stringValue = value?.ToString()?.Trim();

            return stringValue switch
            {
                "مسودة" => CaseStatus.Draft,
                "معلق" => CaseStatus.Pending,
                "قيد المعالجة" => CaseStatus.Processing,
                "مكتمل" => CaseStatus.Complete,
                "ملغي" => CaseStatus.Cancelled,
                _ => throw new ArgumentException($"قيمة الحالة غير معروفة: {stringValue}")
            };
        }
    }
    public class CaseStatusJsonConverter : JsonConverter<CaseStatus>
    {
        public override CaseStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            return value switch
            {
                "مسودة" => CaseStatus.Draft,
                "معلق" => CaseStatus.Pending,
                "قيد المعالجة" => CaseStatus.Processing,
                "مكتمل" => CaseStatus.Complete,
                "ملغي" => CaseStatus.Cancelled,
                _ => throw new JsonException($"قيمة غير صالحة: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, CaseStatus value, JsonSerializerOptions options)
        {
            string stringValue = value switch
            {
                CaseStatus.Draft => "مسودة",
                CaseStatus.Pending => "معلق",
                CaseStatus.Processing => "قيد المعالجة",
                CaseStatus.Complete => "مكتمل",
                CaseStatus.Cancelled => "ملغي",
                _ => throw new JsonException($"قيمة غير معروفة: {value}")
            };

            writer.WriteStringValue(stringValue);
        }
    }
}
