namespace Destry.Http.Data;

/// <summary>
///     Use to mark method parameter to become a content of HTTP Path Parameter.
/// </summary>
/// <example>
///     <code>
///         [SendGet("user/{id}")]
///         Task&lt;User&gt; GetUser([Param] int id);
///     </code>
/// </example>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public sealed class ParamAttribute : PrimitiveDataAttribute
{
    /// <summary>
    ///     Default Param creation, that will parse field name to send it to the server.
    /// </summary>
    /// <example>
    ///     <code>
    ///         [SendPost("api/{value}")]
    ///         Task&lt;ServerResponse&gt; DoSomeStuff([Param] int value);
    ///     </code>
    /// </example>
    public ParamAttribute() { }

    /// <summary>
    ///     Param Creation with specified name to send it to the server.
    /// </summary>
    /// <example>
    ///     <code>
    ///         [SendPost("api/{trivago}")]
    ///         Task&lt;ServerResponse&gt; DoSomeStuff([Param("trivago")] int value);
    ///     </code>
    /// </example>
    /// <param name="name">Name of value that will provided in request.</param>
    public ParamAttribute(string name) : base(name) { }

    internal override HttpSender ApplyData(
        HttpSender httpSender,
        string? key = "null",
        object? data = null)
    {
        if (data is null) return httpSender;

        var type = data.GetType();

        if (type.IsPrimitive)
        {
            var dataValue = Convert.ChangeType(data, typeof(string)) as string;
            httpSender.AddParam(FieldName ?? key ?? "null", dataValue ?? "null");

            return httpSender;
        }

        if (type.IsClass)
        {
            //TODO: warning log - type is not primitive, FieldName ignored

            var queries = GetStringsRecursively(data);
            foreach (var query in queries) httpSender.AddParam(query.Key, query.Value);

            return httpSender;
        }

        return httpSender;
    }
}
