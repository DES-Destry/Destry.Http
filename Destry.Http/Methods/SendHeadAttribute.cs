namespace Destry.Http.Methods;

/// <summary>
///     Send a HEAD request.
/// </summary>
/// <param name="resource">An HTTP method's path to resource.</param>
[AttributeUsage(AttributeTargets.Method)]
public sealed class SendHeadAttribute(string resource) : SendAttribute(resource)
{
    internal override HttpMethod Method => HttpMethod.Head;
}
