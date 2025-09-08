using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LOMs.Domain.Cases.Enums
{
    [JsonConverter(typeof(ContractTypeJsonConverter))]
    public enum ContractType
    {
        FixedTerm = 1,   // محدد المدة
        OpenEnded = 2    // غير محدد المدة
    }

    public class ContractTypeJsonConverter : JsonConverter<ContractType>
    {
        public override ContractType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            return value switch
            {
                "محدد المدة" => ContractType.FixedTerm,
                "غير محدد المدة" => ContractType.OpenEnded,
                _ => throw new JsonException($"قيمة غير صالحة: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, ContractType value, JsonSerializerOptions options)
        {
            string stringValue = value switch
            {
                ContractType.FixedTerm => "محدد المدة",
                ContractType.OpenEnded => "غير محدد المدة",
                _ => throw new JsonException($"قيمة غير معروفة: {value}")
            };

            writer.WriteStringValue(stringValue);
        }
    }
}
