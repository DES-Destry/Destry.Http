using System.Reflection;
using Destry.Http.Controller;
using Destry.Http.Proxies;
using Destry.Http.Senders;

namespace Destry.Http;

public sealed class ControllerBuilder
{
    private string? _baseUrl;
    private Sender _sender = new HttpClientSender();

    public ControllerBuilder WithBaseUrl(string url)
    {
        _baseUrl = url;
        return this;
    }

    public ControllerBuilder WithSender(Sender sender)
    {
        _sender = sender;
        return this;
    }

    public T From<T>() where T : class
    {
        var baseUrl = _baseUrl;
        var type = typeof(T);

        var controllerAttribute = type.GetCustomAttribute<ControllerAttribute>(false);

        if (controllerAttribute?.BaseUrl is not null)
            baseUrl = controllerAttribute.BaseUrl;

        if (baseUrl is null)
            throw new NullReferenceException(nameof(baseUrl));

        var proxy = DispatchProxy.Create<T, ControllerProxy<T>>();
        ((ControllerProxy<T>) (object) proxy).Initialize(baseUrl, _sender);

        return proxy;
    }
}
