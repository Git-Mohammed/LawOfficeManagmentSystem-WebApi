using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LOMs.Domain.Cases.Judgments
{
    [JsonConverter(typeof(JudgmentSubTypeJsonConverter))]
    [TypeConverter(typeof(JudgmentSubTypeTypeConverter))]
    public enum JudgmentSubType
    {
        [Display(Name = "غير محدد")]
        None = 0,

        [Display(Name = "مميز")]
        Vip = 1,

        [Display(Name = "عادي")]
        Normal = 2
    }

    public class JudgmentSubTypeTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            => sourceType == typeof(string);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            var stringValue = value?.ToString()?.Trim();

            return stringValue switch
            {
                "غير محدد" => JudgmentSubType.None,
                "مميز" => JudgmentSubType.Vip,
                "عادي" => JudgmentSubType.Normal,
                _ => throw new ArgumentException($"نوع الصك الفرعي غير معروف: {stringValue}")
            };
        }
    }

    public class JudgmentSubTypeJsonConverter : JsonConverter<JudgmentSubType>
    {
        public override JudgmentSubType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            return value switch
            {
                "غير محدد" => JudgmentSubType.None,
                "مميز" => JudgmentSubType.Vip,
                "عادي" => JudgmentSubType.Normal,
                _ => throw new JsonException($"قيمة غير صالحة: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, JudgmentSubType value, JsonSerializerOptions options)
        {
            string stringValue = value switch
            {
                JudgmentSubType.None => "غير محدد",
                JudgmentSubType.Vip => "مميز",
                JudgmentSubType.Normal => "عادي",
                _ => throw new JsonException($"قيمة غير معروفة: {value}")
            };

            writer.WriteStringValue(stringValue);
        }
    }
}
