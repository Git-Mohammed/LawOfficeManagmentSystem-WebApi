using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LOMs.Contract.Commons.Enums
{
    [JsonConverter(typeof(cJudgmentTypeJsonConverter))]
    [TypeConverter(typeof(cJudgmentTypeTypeConverter))]
    public enum JudgmentType
    {
        [Display(Name = "غير محدد")]
        None = 0,

        [Display(Name = "درجة اولى")]
        JudgmentOne = 1,

        [Display(Name = "درجة ثانية")]
        JudgmentTwo = 2,

        [Display(Name = "درجة ثالثة")]
        JudgmentThree = 3,

        [Display(Name = "درجة رابعة")]
        JudgmentFour = 4
    }

    public class cJudgmentTypeTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            => sourceType == typeof(string);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            var stringValue = value?.ToString()?.Trim();

            return stringValue switch
            {
                "غير محدد" => JudgmentType.None,
                "درجة اولى" => JudgmentType.JudgmentOne,
                "درجة ثانية" => JudgmentType.JudgmentTwo,
                "درجة ثالثة" => JudgmentType.JudgmentThree,
                "درجة رابعة" => JudgmentType.JudgmentFour,
                _ => throw new ArgumentException($"نوع الصك غير معروف: {stringValue}")
            };
        }
    }

    public class cJudgmentTypeJsonConverter : JsonConverter<JudgmentType>
    {
        public override JudgmentType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            return value switch
            {
                "غير محدد" => JudgmentType.None,
                "درجة اولى" => JudgmentType.JudgmentOne,
                "درجة ثانية" => JudgmentType.JudgmentTwo,
                "درجة ثالثة" => JudgmentType.JudgmentThree,
                "درجة رابعة" => JudgmentType.JudgmentFour,
                _ => throw new JsonException($"قيمة غير صالحة: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, JudgmentType value, JsonSerializerOptions options)
        {
            string stringValue = value switch
            {
                JudgmentType.None => "غير محدد",
                JudgmentType.JudgmentOne => "درجة اولى",
                JudgmentType.JudgmentTwo => "درجة ثانية",
                JudgmentType.JudgmentThree => "درجة ثالثة",
                JudgmentType.JudgmentFour => "درجة رابعة",
                _ => throw new JsonException($"قيمة غير معروفة: {value}")
            };

            writer.WriteStringValue(stringValue);
        }
    }
}
