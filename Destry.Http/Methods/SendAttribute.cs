namespace Destry.Http.Methods;

[AttributeUsage(AttributeTargets.Method)]
public abstract class SendAttribute : Attribute
{
    public string Path { get; init; }

    internal SendAttribute(string path)
    {
        Path = path;
    }
}
