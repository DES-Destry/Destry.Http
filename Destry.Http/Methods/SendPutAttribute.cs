namespace Destry.Http.Methods;

[AttributeUsage(AttributeTargets.Method)]
public sealed class SendPutAttribute(string path) : SendAttribute(path)
{
    public override HttpMethod Method => HttpMethod.Put;
}
