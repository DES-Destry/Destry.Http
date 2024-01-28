namespace Destry.Http.Data;

/// <summary>
///     Some primitive data types can provide name from their own names, but mostly user would like to overwrite it.
/// </summary>
public abstract class PrimitiveDataAttribute : DataAttribute
{
    /// <summary>
    ///     Default creation, that will parse field name to send it to the server
    /// </summary>
    /// <example>
    ///     <code>
    ///         // Will send query like: ?value={content of value parameter}
    ///         [SendPost("api")]
    ///         Task&lt;ServerResponse&gt; DoSomeStuff([Query] int value);
    ///     </code>
    /// </example>
    protected PrimitiveDataAttribute() { }

    /// <summary>
    ///     Creation, with specified name to send it to the server
    /// </summary>
    /// <example>
    ///     <code>
    ///         // Will send query like: ?trivago={content of value parameter}
    ///         [SendPost("api")]
    ///         Task&lt;ServerResponse&gt; DoSomeStuff([Query("trivago")] int value);
    ///     </code>
    /// </example>
    protected PrimitiveDataAttribute(string fieldName) { FieldName = fieldName; }

    internal string? FieldName { get; set; }
}
