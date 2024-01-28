namespace Destry.Http.Methods;

/// <summary>
///     Send a PATCH request.
/// </summary>
/// <param name="resource">An HTTP method's path to resource.</param>
[AttributeUsage(AttributeTargets.Method)]
public sealed class SendPatchAttribute(string resource) : SendAttribute(resource)
{
    internal override HttpMethod Method => HttpMethod.Patch;
}
