namespace Destry.Http.Methods;

[AttributeUsage(AttributeTargets.Method)]
public sealed class SendGetAttribute(string path) : SendAttribute(path)
{
    public override HttpMethod Method => HttpMethod.Get;
}
