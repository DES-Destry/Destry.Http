using System.Text.Json;

namespace Destry.Http.Converters;

internal sealed class JsonConverter : Converter
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public override async Task<T?> FromRawResponseToAsync<T>(Task<HttpRawResponse> response)
        where T : default
    {
        var awaited = await response;

        if (!awaited.IsError)
            return await Task.FromResult(JsonSerializer.Deserialize<T>(awaited.Data!, _options));

        if (awaited.IsError)
            throw awaited.Exception!;

        return default;
    }
}
