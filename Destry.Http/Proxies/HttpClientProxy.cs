using System.Reflection;

namespace Destry.Http.Proxies;

internal class HttpClientProxy<T> : DispatchProxy where T : class
{
    private string _baseUrl;

    public void SetBaseUrl(string url) => _baseUrl = url;

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        throw new NotImplementedException();
    }
}
