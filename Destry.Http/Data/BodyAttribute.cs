using Destry.Http.Methods;

namespace Destry.Http.Data;

/// <summary>
///     Use to mark method parameter to become a content of HTTP body.
/// </summary>
/// <remarks>
///     Please, don't use it with <see cref="SendGetAttribute" /> or with <see cref="SendDeleteAttribute" /> :).
///     It shouldn't affect on your request (GET, DELETE method haven't request body).
/// </remarks>
/// <example>
///     <code>
///         [SendPost("user")]
///         Task&lt;ServerResponse&gt; CreateUser([Body] User user);
///     </code>
/// </example>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class BodyAttribute : DataAttribute
{
    internal override HttpSender ApplyData(
        HttpSender httpSender,
        string? key = "null",
        object? data = null)
    {
        httpSender.SetBody(data ?? new { });
        return httpSender;
    }
}
