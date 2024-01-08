using System.Reflection;
using Destry.Http.Data;
using Destry.Http.Methods;

namespace Destry.Http.Proxies;

internal sealed class ControllerProxy<T> : DispatchProxy where T : class
{
    private string? _baseUrl;
    private Sender? _sender;

    public void Initialize(string url, Sender sender)
    {
        _baseUrl = url;
        _sender = sender;
    }

    protected override async Task<object?> Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        ArgumentNullException.ThrowIfNull(targetMethod);
        ArgumentNullException.ThrowIfNull(_baseUrl);
        ArgumentNullException.ThrowIfNull(_sender);

        var sendAttribute = targetMethod.GetCustomAttribute<SendAttribute>(true);

        if (sendAttribute is null)
            throw new Exception(
                "To make request callable you must use any of Send attribute on method"); // TODO: NotSendableMethodException

        var parameters = targetMethod.GetParameters();

        foreach (var (parameter, i) in parameters.Select((parameter, i) => (parameter, i)))
        {
            if (args?[i] is null) break;

            var dataAttributes = parameter.GetCustomAttribute<DataAttribute>();
            _sender = dataAttributes?.ApplyData(_sender, args[i]!) ?? _sender;
        }


        _sender.SetBaseUrl(_baseUrl);
        var returnType = targetMethod.ReturnType;
        var sendHttpRequestMethod =
            _sender.GetType().GetMethod("SendHttpRequestAsync")!.MakeGenericMethod([returnType]);


        //return await _sender.SendHttpRequestAsync<T>(
        // sendAttribute.Method.Method,
        // sendAttribute.Path); - with dynamic T
        return await (Task<object>) sendHttpRequestMethod.Invoke(_sender, [
            sendAttribute.Method.Method,
            sendAttribute.Path
        ])!;
    }
}
