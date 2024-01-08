namespace Destry.Http.Data;

public abstract class PrimitiveDataAttribute : DataAttribute
{
    protected PrimitiveDataAttribute() { }
    protected PrimitiveDataAttribute(string fieldName) { FieldName = fieldName; }

    public string? FieldName { get; set; }
}
