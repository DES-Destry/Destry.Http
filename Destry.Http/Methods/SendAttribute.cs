using System.Diagnostics.CodeAnalysis;

namespace Destry.Http.Methods;

/// <summary>
///     Base attribute to create implementation of HTTP methods calls.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public abstract class SendAttribute : Attribute
{
    internal SendAttribute([StringSyntax("Route")] string resource) { Resource = resource; }

    internal abstract HttpMethod Method { get; }
    internal string Resource { get; init; }
}
