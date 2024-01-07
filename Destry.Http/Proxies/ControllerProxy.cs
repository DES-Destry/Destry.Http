using System.Reflection;

namespace Destry.Http.Proxies;

internal class ControllerProxy<T> : DispatchProxy where T : class
{
    private string _baseUrl;
    private Sender _sender;

    public void Initialize(string url, Sender sender)
    {
        _baseUrl = url;
        _sender = sender;
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        throw new NotImplementedException();
    }
}
