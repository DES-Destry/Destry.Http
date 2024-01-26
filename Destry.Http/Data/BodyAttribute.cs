namespace Destry.Http.Data;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class BodyAttribute : DataAttribute
{
    public override HttpSender ApplyData(HttpSender httpSender, object data)
    {
        httpSender.SetBody(data);
        return httpSender;
    }
}
