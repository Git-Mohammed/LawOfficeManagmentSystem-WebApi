using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LOMs.Contract.Commons.Enums;

[JsonConverter(typeof(cContractTypeJsonConverter))]
[TypeConverter(typeof(cContractTypeConverter))]
public enum ContractType
{
    [Display(Name = "محدد بمدة")]
    FixedTerm = 1,
    [Display(Name = "غير محدد بمدة")]
    OpenEnded = 2
}
public class cContractTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        var stringValue = value?.ToString()?.Trim();

        return stringValue switch
        {
            "محدد بمدة" => ContractType.FixedTerm,
            "غير محدد بمدة" => ContractType.OpenEnded,
            _ => throw new ArgumentException($"قيمة الحالة غير معروفة: {stringValue}")
        };
    }
}
public class cContractTypeJsonConverter : JsonConverter<ContractType>
{
    public override ContractType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        return value switch
        {
            "محدد بمدة" => ContractType.FixedTerm,
            "غير محدد بمدة" => ContractType.OpenEnded,
            _ => throw new JsonException($"قيمة غير صالحة: {value}")
        };
    }

    public override void Write(Utf8JsonWriter writer, ContractType value, JsonSerializerOptions options)
    {
        string stringValue = value switch
        {
            ContractType.FixedTerm => "محدد بمدة",
            ContractType.OpenEnded => "غير محدد بمدة",
            _ => throw new JsonException($"قيمة غير معروفة: {value}")
        };

        writer.WriteStringValue(stringValue);
    }
}