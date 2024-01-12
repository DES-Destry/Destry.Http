namespace Destry.Http;

public abstract class Converter
{
    public abstract Task<T> FromRawResponseToAsync<T>(HttpRawResponse response);
}
