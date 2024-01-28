namespace Destry.Http.Data;

/// <summary>
///     Define request header in controller.
/// </summary>
public sealed class WithHeaderAttribute : KeyValueDataAttribute
{
    /// <summary>
    ///     Set header value.
    /// </summary>
    /// <param name="key">Key of the header.</param>
    /// <param name="value">Value of the header.</param>
    /// <example>
    ///     <code>
    ///         [WithHeader("ngrok-skip-browser-warning", "true")]
    ///         [SendGet("ngrok")]
    ///         Task&lt;ServerResponse&gt; RequestNgrok();
    ///     </code>
    /// </example>
    public WithHeaderAttribute(string key, string value) : base(key, value) { }

    internal override HttpSender ApplyData(HttpSender httpSender, object? data)
    {
        httpSender.AddHeader(Key, Value);
        return httpSender;
    }
}
