using System.Text.Json;
using System.Text.Json.Serialization;

namespace Packtastic.Avalonia.Core.Helpers;

public class InterfaceConverter<TInterface, TImplementation> : JsonConverter<TInterface>
    where TImplementation : TInterface, new()
{
    public override TInterface Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var implementation = JsonSerializer.Deserialize<TImplementation>(ref reader, options);
        return implementation;
    }

    public override void Write(Utf8JsonWriter writer, TInterface value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (TImplementation)value, options);
    }
}