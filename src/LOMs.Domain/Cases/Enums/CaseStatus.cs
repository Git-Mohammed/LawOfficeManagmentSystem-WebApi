using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LOMs.Domain.Cases.Enums
{
    [JsonConverter(typeof(CaseStatusJsonConverter))]
    [TypeConverter(typeof(CaseStatusTypeConverter))]
    public enum CaseStatus
    {
        [Display(Name = "مسودة")]
        Draft = 0,

        [Display(Name = "قيد نظر")]
        Pending = 1,

        [Display(Name = "قيد تنفيذ")]
        Processing = 2,

        [Display(Name = "قيد منتهي")]
        Done = 3,

        [Display(Name = "ملغي")]
        Cancelled = 4
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
                "قيد نظر" => CaseStatus.Pending,
                "قيد تنفيذ" => CaseStatus.Processing,
                "قيد منتهي" => CaseStatus.Done,
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
                "قيد نظر" => CaseStatus.Pending,
                "قيد تنفيذ" => CaseStatus.Processing,
                "قيد منتهي" => CaseStatus.Done,
                "ملغي" => CaseStatus.Cancelled,
                _ => throw new JsonException($"قيمة غير صالحة: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, CaseStatus value, JsonSerializerOptions options)
        {
            string stringValue = value switch
            {
                CaseStatus.Draft => "مسودة",
                CaseStatus.Pending => "قيد نظر",
                CaseStatus.Processing => "قيد تنفيذ",
                CaseStatus.Done => "قيد منتهي",
                CaseStatus.Cancelled => "ملغي",
                _ => throw new JsonException($"قيمة غير معروفة: {value}")
            };

            writer.WriteStringValue(stringValue);
        }
    }
}
