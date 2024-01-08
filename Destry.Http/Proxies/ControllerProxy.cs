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

        _sender.Reset();

        var sendAttribute = targetMethod.GetCustomAttribute<SendAttribute>(true);

        if (sendAttribute is null)
            throw new NotCallableMethodException(targetMethod);

        var parameters = targetMethod.GetParameters();

        foreach (var (parameter, i) in parameters.Select((parameter, i) => (parameter, i)))
        {
            if (args?[i] is null) break;

            var dataAttributes = parameter.GetCustomAttribute<DataAttribute>();
            _sender = dataAttributes?.ApplyData(_sender, args[i]!) ?? _sender;
        }


        _sender.SetBaseUrl(_baseUrl);
        var returnTask = targetMethod.ReturnType;
        var returnTypes = returnTask.GetGenericArguments();

        if (!returnTask.Name.Contains("Task") && returnTask.GetGenericArguments().Length == 0)
            throw new Exception("Method must return Task<?> type.");

        var sendHttpRequestMethod =
            _sender.GetType().GetMethod("SendHttpRequestAsync")!
                .MakeGenericMethod([returnTypes[0]]);

        return sendHttpRequestMethod.Invoke(_sender, [
            sendAttribute.Method.Method,
            sendAttribute.Path
        ])!;
    }
}
