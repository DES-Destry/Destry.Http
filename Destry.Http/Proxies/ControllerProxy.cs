using System.Reflection;
using Destry.Http.Data;
using Destry.Http.Exceptions;
using Destry.Http.Methods;

namespace Destry.Http.Proxies;

internal class ControllerProxy<T> : DispatchProxy where T : class
{
    private string? _baseUrl;
    private Sender? _sender;

    public void Initialize(string url, Sender sender)
    {
        _baseUrl = url;
        _sender = sender;
    }

    protected override dynamic Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        ArgumentNullException.ThrowIfNull(targetMethod);
        ArgumentNullException.ThrowIfNull(_baseUrl);
        ArgumentNullException.ThrowIfNull(_sender);

        var sender = _sender.NewInstance();

        var sendAttribute = targetMethod.GetCustomAttribute<SendAttribute>(true);

        if (sendAttribute is null)
            throw new NotCallableMethodException(targetMethod);

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
        var returnType = targetMethod.ReturnType;
        var returnTypes = returnType.GetGenericArguments();

        if (returnType.Name.Contains("Task") && returnType.GetGenericArguments().Length != 0)
            returnType = returnTypes[0];

        var sendHttpRequestMethod =
            sender.GetType().GetMethod("SendHttpRequestAsync")!
                .MakeGenericMethod([returnType]);

        return sendHttpRequestMethod.Invoke(sender, [
            sendAttribute.Method.Method,
            sendAttribute.Path
        ])!;
    }
}
