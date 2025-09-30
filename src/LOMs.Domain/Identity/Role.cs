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
        [Display(Name = "المدير العام")]
        GeneralManager = 1,

        [Display(Name = "المدير التنفيذي")]
        ExecutiveManager = 2,

        [Display(Name = "المستشار")]
        Consultant = 3,

        [Display(Name = "المحامي")]
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
                "المدير العام" => Role.GeneralManager,
                "المدير التنفيذي" => Role.ExecutiveManager,
                "المستشار" => Role.Consultant,
                "المحامي" => Role.Lawyer,
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
                "المدير العام" => Role.GeneralManager,
                "المدير التنفيذي" => Role.ExecutiveManager,
                "المستشار" => Role.Consultant,
                "المحامي" => Role.Lawyer,
                _ => throw new JsonException($"قيمة غير صالحة: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, Role value, JsonSerializerOptions options)
        {
            string stringValue = value switch
            {
                Role.GeneralManager => "المدير العام",
                Role.ExecutiveManager => "المدير التنفيذي",
                Role.Consultant => "المستشار",
                Role.Lawyer => "المحامي",
                _ => throw new JsonException($"قيمة غير معروفة: {value}")
            };

            writer.WriteStringValue(stringValue);
        }
    }
}
