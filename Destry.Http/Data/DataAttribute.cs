using System.Reflection;

namespace Destry.Http.Data;

[AttributeUsage(AttributeTargets.Parameter)]
public abstract class DataAttribute : Attribute
{
    public abstract Sender ApplyData(Sender sender, object data);

    protected static Dictionary<string, string> GetStringsRecursively(
        object? data,
        int level = 1)
    {
        Dictionary<string, string> result = [];
        const int maxDeep = 10;

        if (level >= maxDeep || data is null) return result;

        var type = data.GetType();
        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            if (property.PropertyType.IsPrimitive)
            {
                var attribute = property.GetCustomAttribute<PrimitiveDataAttribute>(true);
                var dataValue =
                    Convert.ChangeType(property.GetValue(data), typeof(string)) as string;

                //TODO: change case for property.Name
                result.TryAdd(attribute?.FieldName ?? property.Name, dataValue ?? "null");

                continue;
            }

            if (property.PropertyType.IsClass)
            {
                var nestedStrings = GetStringsRecursively(property.GetValue(data), level++);
                foreach (var nestedString in nestedStrings)
                    result.TryAdd(nestedString.Key, nestedString.Value);
            }
        }

        return result;
    }
}
