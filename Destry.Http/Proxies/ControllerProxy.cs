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
                    {
                        _sender.AddQuery(queryAttribute.FieldName ?? parameter.Name ?? "", arg);
                        continue;
                    }

                    throw new ArgumentException("[Query] can be applied only on string type.");
                }

                if (dataAttribute is ParamAttribute paramAttribute)
                {
                    if (args[i] is string arg)
                    {
                        _sender.AddParam(paramAttribute.FieldName ?? parameter.Name ?? "", arg);
                        continue;
                    }

                    throw new ArgumentException("[Param] can be applied only on string type.");
                }

                if (dataAttribute is QueriesAttribute)
                {
                    var queries = GetStringsRecursively(args[i]);
                    foreach (var query in queries) _sender.AddQuery(query.Key, query.Value);
                }

                if (dataAttribute is ParamsAttribute)
                {
                    var @params = GetStringsRecursively(args[i]);
                    foreach (var param in @params) _sender.AddParam(param.Key, param.Value);
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

    private Dictionary<string, string> GetStringsRecursively(
        object? obj,
        int level = 1)
    {
        Dictionary<string, string> result = [];
        const int maxDeep = 10;

        if (level >= maxDeep || obj is null) return result;

        var type = obj.GetType();
        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            if (property.PropertyType.IsPrimitive)
            {
                var attribute = property.GetCustomAttribute<DataAttribute>(true);

                //TODO: change case for property.Name
                result.Add(attribute?.FieldName ?? property.Name,
                    (property.GetValue(obj) as string)!);

                continue;
            }

            if (property.PropertyType.IsClass)
            {
                var nestedStrings = GetStringsRecursively(property.GetValue(obj), level++);
                foreach (var nestedString in nestedStrings)
                    result.Add(nestedString.Key, nestedString.Value);
            }
        }

        return result;
    }
}
