namespace Destry.Http.Data;

/// <summary>
///     Base class for creating attributes that will attach to method or even to all controller interface with specified
///     values on compilation.
/// </summary>
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method)]
public abstract class KeyValueDataAttribute : DataAttribute
{
    /// <summary>
    ///     Create key-value data attribute.
    /// </summary>
    /// <param name="key">Key of the data.</param>
    /// <param name="value">Value of the data.</param>
    protected KeyValueDataAttribute(string key, string value)
    {
        Key = key;
        Value = value;
    }

    internal string Key { get; }
    internal string Value { get; }
}
