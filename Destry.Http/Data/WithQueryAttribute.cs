namespace Destry.Http.Data;

/// <summary>
///     Define request query in controller.
/// </summary>
public class WithQueryAttribute : KeyValueDataAttribute
{
    /// <summary>
    ///     Set query value.
    /// </summary>
    /// <param name="key">Key of the query.</param>
    /// <param name="value">Value of the query.</param>
    /// <example>
    ///     <code>
    ///         [WithQuery("sort", "DESC")]
    ///         [SendGet("users")]
    ///         Task&lt;IEnumerable&lt;User&gt;&gt; GetUsers();
    ///     </code>
    /// </example>
    public WithQueryAttribute(string key, string value) : base(key, value) { }

    internal override HttpSender ApplyData(HttpSender httpSender, object? data)
    {
        httpSender.AddQuery(Key, Value);
        return httpSender;
    }
}
