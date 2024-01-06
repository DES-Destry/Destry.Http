namespace Destry.Http.Data;

[AttributeUsage(AttributeTargets.Parameter)]
public abstract class DataAttribute : Attribute
{
    public string? FieldName { get; set; }

    protected DataAttribute() { }
    protected DataAttribute(string fieldName)
    {
        FieldName = fieldName;
    }
}
