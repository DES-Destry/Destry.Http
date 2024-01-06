namespace Destry.Http.Controller;

[AttributeUsage(AttributeTargets.Interface)]
public sealed class ControllerAttribute : Attribute
{
    public string? BaseUrl { get; set; }
    
    public ControllerAttribute() { }
    public ControllerAttribute(string baseUrl)
    {
        BaseUrl = baseUrl;
    }
}
