using System.Reflection;
using Destry.Http.Methods;

namespace Destry.Http.Proxies;

internal sealed class ControllerProxy<T> : DispatchProxy where T : class
{
    private string _baseUrl;
    private Sender _sender;

    public void Initialize(string url, Sender sender)
    {
        _baseUrl = url;
        _sender = sender;
    }

    protected override async Task<object?> Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        ArgumentNullException.ThrowIfNull(targetMethod);

        var sendAttribute = targetMethod.GetCustomAttribute<SendAttribute>(true);

        if (sendAttribute is null)
            throw new Exception(
                "To make request callable you must use any of Send attribute on method"); // TODO: NotSendableMethodException

        var returnType = targetMethod.ReturnType;

        var sendHttpRequestMethod =
            _sender.GetType().GetMethod("SendHttpRequestAsync")!.MakeGenericMethod([returnType]);


        //return await _sender.SendHttpRequestAsync<T>(
        // sendAttribute.Method.Method,
        // sendAttribute.Path); - with dynamic T
        return sendHttpRequestMethod.Invoke(_sender, [
            sendAttribute.Method.Method,
            sendAttribute.Path
        ]);
    }
}
