namespace Destry.Http.Data;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class BodyAttribute : DataAttribute
{
    public override Sender ApplyData(Sender sender, object data)
    {
        sender.SetBody(data);
        return sender;
    }
}
