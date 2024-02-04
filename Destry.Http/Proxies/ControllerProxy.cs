using System.Reflection;
using Destry.Http.Data;
using Destry.Http.Exceptions;
using Destry.Http.Methods;

namespace Destry.Http.Proxies;

internal class ControllerProxy<T> : DispatchProxy where T : class
{
    private string? _baseUrl;
    private IEnumerable<KeyValueDataAttribute> _controllerData = [];
    private Converter? _converter;
    private HttpSender? _sender;

    public void Initialize(
        string url,
        Converter converter,
        HttpSender httpSender,
        IEnumerable<KeyValueDataAttribute> controllerData)
    {
        _baseUrl = url;
        _converter = converter;
        _sender = httpSender;
        _controllerData = controllerData;
    }

    protected override object Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        ArgumentNullException.ThrowIfNull(targetMethod);
        ArgumentNullException.ThrowIfNull(_baseUrl);
        ArgumentNullException.ThrowIfNull(_sender);
        ArgumentNullException.ThrowIfNull(_converter);

        var sender = _sender.NewInstance();

        var sendAttribute = targetMethod.GetCustomAttribute<SendAttribute>(true);

        if (sendAttribute is null)
            throw new NotCallableMethodException(targetMethod);

        var keyValueDataAttributes = targetMethod.GetCustomAttributes<KeyValueDataAttribute>(true);

        // Apply attributes from method like [WithHeader] or [WithQuery]
        sender = _controllerData.Aggregate(sender,
            (current, keyValueData) => keyValueData.ApplyData(current));
        sender = keyValueDataAttributes.Aggregate(sender,
            (current, keyValueData) => keyValueData.ApplyData(current));

        var parameters = targetMethod.GetParameters();

        foreach (var (parameter, i) in parameters.Select((parameter, i) => (parameter, i)))
        {
            if (args?[i] is null) break;

            var excludeAttribute = parameter.GetCustomAttribute<ExcludeFromRequestAttribute>();
            if (excludeAttribute is not null) continue;

            var dataAttributes = parameter.GetCustomAttribute<DataAttribute>();
            sender = dataAttributes?.ApplyData(sender, args[i]!) ?? sender;
        }


        sender.SetBaseUrl(_baseUrl);
        return SendRequestAndConvertAsync(targetMethod, sender, sendAttribute);
    }

    private object SendRequestAndConvertAsync(
        MethodInfo targetMethod,
        HttpSender httpSender,
        SendAttribute sendAttribute)
    {
        var response =
            httpSender.SendHttpRequestAsync(sendAttribute.Method.Method, sendAttribute.Resource);

        var returnType = targetMethod.ReturnType;
        var returnTypes = returnType.GetGenericArguments();

        if (returnType.Name.Contains("Task") && returnType.GetGenericArguments().Length != 0)
            returnType = returnTypes[0];

        var fromResponseToDataMethod =
            _converter?.GetType().GetMethod("FromRawResponseToAsync")!
                .MakeGenericMethod([returnType]);

        return fromResponseToDataMethod?.Invoke(_converter, [response])!;
    }
}
