namespace Destry.Http.Data;

/// <summary>
///     Exclude value from request. Useful if parameter with class type was marked with any <see cref="DataAttribute" />
///     and some values you don't want to send.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public class ExcludeFromRequestAttribute : Attribute
{
}
