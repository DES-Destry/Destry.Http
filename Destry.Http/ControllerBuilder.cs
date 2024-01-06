using System.Reflection;
using Destry.Http.Controller;
using Destry.Http.Proxies;

namespace Destry.Http;

public sealed class ControllerBuilder
{
    private string? _baseUrl;

    public ControllerBuilder WithBaseUrl(string url)
    {
        _baseUrl = url;
        return this;
    }

    public T InstanceOf<T>() where T : class
    {
        var baseUrl = _baseUrl;
        var type = typeof(T);

        if (type.GetCustomAttribute(typeof(ControllerAttribute), false) is not ControllerAttribute
            controllerAttribute)
            throw new Exception();

        if (controllerAttribute.BaseUrl is not null)
            baseUrl = controllerAttribute.BaseUrl;

        if (baseUrl is null)
            throw new NullReferenceException(nameof(baseUrl));

        var proxy = DispatchProxy.Create<T, HttpClientProxy<T>>();
        ((HttpClientProxy<T>) (object) proxy).SetBaseUrl(baseUrl);

        return proxy;
    }
}
