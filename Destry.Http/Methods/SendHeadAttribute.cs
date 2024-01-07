namespace Destry.Http.Methods;

[AttributeUsage(AttributeTargets.Method)]
public sealed class SendHeadAttribute(string path) : SendAttribute(path)
{
    public override HttpMethod Method => HttpMethod.Head;
}
