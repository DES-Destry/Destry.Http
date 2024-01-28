namespace Destry.Http.Methods;

/// <summary>
///     Send a GET request.
/// </summary>
/// <param name="resource">An HTTP method's path to resource.</param>
[AttributeUsage(AttributeTargets.Method)]
public sealed class SendGetAttribute(string resource) : SendAttribute(resource)
{
    internal override HttpMethod Method => HttpMethod.Get;
}
