namespace Destry.Http.Data;

/// <summary>
///     Some primitive data types can provide name from their own names, but mostly user would like to overwrite it.
/// </summary>
public abstract class PrimitiveDataAttribute : DataAttribute
{
    /// <summary>
    ///     Default creation, that will parse field name to send it to the server.
    /// </summary>
    protected PrimitiveDataAttribute() { }

    /// <summary>
    ///     Creation with specified name to send it to the server
    /// </summary>
    protected PrimitiveDataAttribute(string fieldName) { FieldName = fieldName; }

    internal string? FieldName { get; private set; }
}
