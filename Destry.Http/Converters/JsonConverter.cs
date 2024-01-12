namespace Destry.Http.Converters;

internal sealed class JsonConverter : Converter
{
    public override Task<T> FromRawResponseToAsync<T>(HttpRawResponse response)
    {
        throw new NotImplementedException();
    }
}
