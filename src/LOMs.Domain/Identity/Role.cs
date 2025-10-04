using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LOMs.Domain.Identity
{
    [JsonConverter(typeof(RoleJsonConverter))]
    [TypeConverter(typeof(RoleTypeConverter))]
    public enum Role
    {
        [Display(Name = "مدير عام")]
        GeneralManager = 1,

        [Display(Name = "مدير تنفيذي")]
        ExecutiveManager = 2,

        [Display(Name = "مستشار")]
        Consultant = 3,

        [Display(Name = "محامي")]
        Lawyer = 4
    }

    public class RoleTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            => sourceType == typeof(string);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            var stringValue = value?.ToString()?.Trim();

            return stringValue switch
            {
                "مدير عام" => Role.GeneralManager,
                "مدير تنفيذي" => Role.ExecutiveManager,
                "مستشار" => Role.Consultant,
                "محامي" => Role.Lawyer,
                _ => throw new ArgumentException($"دور غير معروف: {stringValue}")
            };
        }
    }

    public class RoleJsonConverter : JsonConverter<Role>
    {
        public override Role Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            return value switch
            {
                "مدير عام" => Role.GeneralManager,
                "مدير تنفيذي" => Role.ExecutiveManager,
                "مستشار" => Role.Consultant,
                "محامي" => Role.Lawyer,
                _ => throw new JsonException($"قيمة غير صالحة: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, Role value, JsonSerializerOptions options)
        {
            string stringValue = value switch
            {
                Role.GeneralManager => "مدير عام",
                Role.ExecutiveManager => "مدير تنفيذي",
                Role.Consultant => "مستشار",
                Role.Lawyer => "محامي",
                _ => throw new JsonException($"قيمة غير معروفة: {value}")
            };

            writer.WriteStringValue(stringValue);
        }
    }
}
