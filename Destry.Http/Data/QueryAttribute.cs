namespace Destry.Http.Data;

/// <summary>
///     Use to mark method parameter to become a content of HTTP Query.
/// </summary>
/// <example>
///     <code>
///         // GET /user?id={content of id} will send.
///         [SendGet("user")]
///         Task&lt;User&gt; GetUser([Query] int id);
///     </code>
/// </example>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public sealed class QueryAttribute : PrimitiveDataAttribute
{
    /// <summary>
    ///     Default Query creation, that will parse field name to send it to the server.
    /// </summary>
    /// <example>
    ///     <code>
    ///         // Will send query like: ?value={content of value parameter}
    ///         [SendPost("api")]
    ///         Task&lt;ServerResponse&gt; DoSomeStuff([Query] int value);
    ///     </code>
    /// </example>
    public QueryAttribute() { }

    /// <summary>
    ///     Query Creation with specified name to send it to the server.
    /// </summary>
    /// <example>
    ///     <code>
    ///         // Will send query like: ?trivago={content of value parameter}
    ///         [SendPost("api")]
    ///         Task&lt;ServerResponse&gt; DoSomeStuff([Query("trivago")] int value);
    ///     </code>
    /// </example>
    /// <param name="name">Name of value that will provided in request.</param>
    public QueryAttribute(string name) : base(name) { }

    internal override HttpSender ApplyData(HttpSender httpSender, object data)
    {
        var type = data.GetType();

        if (type.IsPrimitive)
        {
            var dataValue = Convert.ChangeType(data, typeof(string)) as string;
            httpSender.AddQuery(FieldName ?? type.Name, dataValue ?? "null");

            return httpSender;
        }

        if (type.IsClass)
        {
            //TODO: warning log - type is not primitive, FieldName ignored

            var queries = GetStringsRecursively(data);
            foreach (var query in queries) httpSender.AddQuery(query.Key, query.Value);

            return httpSender;
        }

        return httpSender;
    }
}
