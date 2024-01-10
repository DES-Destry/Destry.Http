namespace Destry.Http.Data;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public sealed class HeaderAttribute : PrimitiveDataAttribute
{
    public override Sender ApplyData(Sender sender, object data)
    {
        var type = data.GetType();

        if (type.IsPrimitive)
        {
            var dataValue = Convert.ChangeType(data, typeof(string)) as string;
            sender.AddHeader(FieldName ?? type.Name, dataValue ?? "null");

            return sender;
        }

        if (type.IsClass)
        {
            //TODO: warning log - type is not primitive, FieldName ignored

            var queries = GetStringsRecursively(data);
            foreach (var query in queries) sender.AddHeader(query.Key, query.Value);

            return sender;
        }

        return sender;
    }
}
