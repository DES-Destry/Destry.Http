namespace Destry.Http.Data;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public sealed class ParamAttribute : PrimitiveDataAttribute
{
    public override HttpSender ApplyData(HttpSender httpSender, object data)
    {
        var type = data.GetType();

        if (type.IsPrimitive)
        {
            var dataValue = Convert.ChangeType(data, typeof(string)) as string;
            httpSender.AddParam(FieldName ?? type.Name, dataValue ?? "null");

            return httpSender;
        }

        if (type.IsClass)
        {
            //TODO: warning log - type is not primitive, FieldName ignored

            var queries = GetStringsRecursively(data);
            foreach (var query in queries) httpSender.AddParam(query.Key, query.Value);

            return httpSender;
        }

        return httpSender;
    }
}
