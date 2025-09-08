using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LOMs.Domain.Cases.Enums
{
    [JsonConverter(typeof(PartyRoleJsonConverter))]
    public enum PartyRole
    {
        [Display(Name = "مدعي")]         // Plaintiff: initiates the legal action
        Plaintiff = 1,

        [Display(Name = "مدعى عليه")]    // Defendant: responds to the legal claim
        Defendant = 2
    }

    public class PartyRoleJsonConverter : JsonConverter<PartyRole>
    {
        public override PartyRole Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            return value switch
            {
                "مدعي" => PartyRole.Plaintiff,
                "مدعى عليه" => PartyRole.Defendant,
                _ => throw new JsonException($"قيمة غير صالحة: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, PartyRole value, JsonSerializerOptions options)
        {
            string stringValue = value switch
            {
                PartyRole.Plaintiff => "مدعي",
                PartyRole.Defendant => "مدعى عليه",
                _ => throw new JsonException($"قيمة غير معروفة: {value}")
            };

            writer.WriteStringValue(stringValue);
        }
    }
}
