using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Amazon.Lambda.Core;

namespace LambdaFunctions.Settings;

public class CustomLambdaSerializer : ILambdaSerializer
{

    public T Deserialize<T>(Stream requestStream)
    {
        var policy = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        using var reader = new StreamReader(requestStream);
        var json = reader.ReadToEnd();
        return JsonSerializer.Deserialize<T>(json, policy);
    }

    public void Serialize<T>(T response, Stream responseStream)
    {
        var policy = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() },
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = JavaScriptEncoder.Create(new TextEncoderSettings(System.Text.Unicode.UnicodeRanges.All))
        };

        var json = JsonNode.Parse(JsonSerializer.Serialize(response, policy));
        var bytes = System.Text.Encoding.UTF8.GetBytes(json.ToJsonString());
        responseStream.Write(bytes, 0, bytes.Length);
    }

}