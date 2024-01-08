using System.Diagnostics.CodeAnalysis;

namespace Destry.Http.Methods;

[AttributeUsage(AttributeTargets.Method)]
public abstract class SendAttribute : Attribute
{
    internal SendAttribute([StringSyntax("Route")] string path) { Path = path; }

    public abstract HttpMethod Method { get; }
    public string Path { get; init; }
}
