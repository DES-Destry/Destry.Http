namespace Destry.Http.Methods;

/// <summary>
///     Send a DELETE request.
/// </summary>
/// <param name="resource">An HTTP method's path to resource.</param>
[AttributeUsage(AttributeTargets.Method)]
public sealed class SendDeleteAttribute(string resource) : SendAttribute(resource)
{
    internal override HttpMethod Method => HttpMethod.Delete;
}
