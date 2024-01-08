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

            var dataAttributes = parameter.GetCustomAttributes(false);

            foreach (var dataAttribute in dataAttributes)
            {
                if (dataAttribute is BodyAttribute)
                    _sender.SetBody(args[i]!);

                if (dataAttribute is QueryAttribute queryAttribute)
                {
                    if (args[i] is string arg)
                        _sender.AddQuery(queryAttribute.FieldName ?? parameter.Name ?? "", arg);

                    throw new ArgumentException("[Query] can be applied only on string type.");
                }

                if (dataAttribute is ParamAttribute paramAttribute)
                {
                    if (args[i] is string arg)
                        _sender.AddParam(paramAttribute.FieldName ?? parameter.Name ?? "", arg);

                    throw new ArgumentException("[Param] can be applied only on string type.");
                }
            }
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
