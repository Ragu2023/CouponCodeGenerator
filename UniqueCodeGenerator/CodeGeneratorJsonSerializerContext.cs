using System.Text.Json.Serialization;

namespace UniqueCodeGenerator
{
    [JsonSerializable(typeof(Dictionary<string, string>))]
    [JsonSerializable(typeof(string[]))]
    internal partial class CodeGeneratorJsonSerializerContext : JsonSerializerContext
    {
    }
}
