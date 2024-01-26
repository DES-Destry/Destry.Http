namespace Destry.Http;

public abstract class Converter
{
    public abstract Task<T?> FromRawResponseToAsync<T>(Task<HttpRawResponse> response);
}
