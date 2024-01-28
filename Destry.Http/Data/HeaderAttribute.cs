namespace Destry.Http.Data;

/// <summary>
///     Use to mark method parameter to become a content of HTTP Header.
/// </summary>
/// <example>
///     <code>
///         [SendPost("open-pentagon")]
///         Task&lt;ServerResponse&gt; DoAuthorizedStuff([Header("Authorization")] string bearerToken);
///     </code>
/// </example>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public sealed class HeaderAttribute : PrimitiveDataAttribute
{
    /// <summary>
    ///     Default Header creation, that will parse field name to send it to the server.
    /// </summary>
    /// <example>
    ///     <code>
    ///         [SendPost("open-pentagon")]
    ///         Task&lt;ServerResponse&gt; DoAuthorizedStuff([Header] string authorization);
    ///     </code>
    /// </example>
    public HeaderAttribute() { }

    /// <summary>
    ///     Query Creation with specified name to send it to the server.
    /// </summary>
    /// <example>
    ///     <code>
    ///         [SendPost("open-pentagon")]
    ///         Task&lt;ServerResponse&gt; DoAuthorizedStuff([Header("Authorization")] string bearerToken);
    ///     </code>
    /// </example>
    /// <param name="name">Name of value that will provided in request.</param>
    public HeaderAttribute(string name) : base(name) { }

    internal override HttpSender ApplyData(HttpSender httpSender, object? data)
    {
        if (data is null) return httpSender;

        var type = data.GetType();

        if (type.IsPrimitive)
        {
            var dataValue = Convert.ChangeType(data, typeof(string)) as string;
            httpSender.AddHeader(FieldName ?? type.Name, dataValue ?? "null");

            return httpSender;
        }

        if (type.IsClass)
        {
            //TODO: warning log - type is not primitive, FieldName ignored

            var queries = GetStringsRecursively(data);
            foreach (var query in queries) httpSender.AddHeader(query.Key, query.Value);

            return httpSender;
        }

        return httpSender;
    }
}
