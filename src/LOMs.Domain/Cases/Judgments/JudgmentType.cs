using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LOMs.Domain.Cases.Judgments
{
    [JsonConverter(typeof(JudgmentTypeJsonConverter))]
    [TypeConverter(typeof(JudgmentTypeTypeConverter))]
    public enum JudgmentType
    {
        [Display(Name = "غير محدد")]
        None = 0,

        [Display(Name = "صك نوع أول")]
        JudgmentOne = 1,

        [Display(Name = "صك نوع ثاني")]
        JudgmentTwo = 2,

        [Display(Name = "صك نوع ثالث")]
        JudgmentThree = 3,

        [Display(Name = "صك نوع رابع")]
        JudgmentFour = 4
    }

    public class JudgmentTypeTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            => sourceType == typeof(string);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            var stringValue = value?.ToString()?.Trim();

            return stringValue switch
            {
                "غير محدد" => JudgmentType.None,
                "صك نوع أول" => JudgmentType.JudgmentOne,
                "صك نوع ثاني" => JudgmentType.JudgmentTwo,
                "صك نوع ثالث" => JudgmentType.JudgmentThree,
                "صك نوع رابع" => JudgmentType.JudgmentFour,
                _ => throw new ArgumentException($"نوع الصك غير معروف: {stringValue}")
            };
        }
    }

    public class JudgmentTypeJsonConverter : JsonConverter<JudgmentType>
    {
        public override JudgmentType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            return value switch
            {
                "غير محدد" => JudgmentType.None,
                "صك نوع أول" => JudgmentType.JudgmentOne,
                "صك نوع ثاني" => JudgmentType.JudgmentTwo,
                "صك نوع ثالث" => JudgmentType.JudgmentThree,
                "صك نوع رابع" => JudgmentType.JudgmentFour,
                _ => throw new JsonException($"قيمة غير صالحة: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, JudgmentType value, JsonSerializerOptions options)
        {
            string stringValue = value switch
            {
                JudgmentType.None => "غير محدد",
                JudgmentType.JudgmentOne => "صك نوع أول",
                JudgmentType.JudgmentTwo => "صك نوع ثاني",
                JudgmentType.JudgmentThree => "صك نوع ثالث",
                JudgmentType.JudgmentFour => "صك نوع رابع",
                _ => throw new JsonException($"قيمة غير معروفة: {value}")
            };

            writer.WriteStringValue(stringValue);
        }
    }
}
