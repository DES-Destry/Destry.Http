using System.Text.Json;

namespace Destry.Http.Converters;

internal sealed class JsonConverter : Converter
{
    public override async Task<T?> FromRawResponseToAsync<T>(HttpRawResponse response)
        where T : default
    {
        return await Task.FromResult(JsonSerializer.Deserialize<T>(response.Data));
    }
}
