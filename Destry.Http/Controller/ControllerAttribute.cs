namespace Destry.Http.Controller;

/// <summary>
///     You can add this attribute to say, that this interface can be used by Destry.Http.
/// </summary>
[AttributeUsage(AttributeTargets.Interface)]
public sealed class ControllerAttribute : Attribute
{
    /// <summary>
    ///     Default Destry.Http controller.
    /// </summary>
    /// <remarks>
    ///     You can delete this attribute without any problems. But I'll recommend to use this to say "hey, this interface is
    ///     using by Destry.Http to build custom Http Client" to other programmers.
    /// </remarks>
    public ControllerAttribute() { }

    /// <summary>
    ///     Create custom Http client with specified base URL.
    /// </summary>
    /// <param name="baseUrl">
    ///     It's a first part of the path of any HTTP request. <br />
    ///     For example: https://api.some-cool-service.com
    /// </param>
    public ControllerAttribute(string baseUrl) { BaseUrl = baseUrl; }

    internal string? BaseUrl { get; private set; }
}
