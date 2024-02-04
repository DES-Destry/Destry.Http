using System.Reflection;

namespace Destry.Http.Data;

/// <summary>
///     Base class for creating attributes that will attach data from methods parameters into HTTP request.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public abstract class DataAttribute : Attribute
{
    internal abstract HttpSender ApplyData(
        HttpSender httpSender,
        string? key = "null",
        object? data = null);

    internal static Dictionary<string, string> GetStringsRecursively(
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
            var excludeAttribute = property.GetCustomAttribute<ExcludeFromRequestAttribute>();
            if (excludeAttribute is not null) continue;

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
