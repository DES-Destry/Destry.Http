using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using Destry.Http.Methods;

namespace Destry.Http.Exceptions;

/// <summary>
///     Called method haven't any <see cref="SendAttribute" />.
/// </summary>
/// <param name="method">Method that haven't any <see cref="SendAttribute" />, but it should.</param>
[SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
public class NotCallableMethodException(MethodInfo method) : Exception(GetMessageFrom(method))
{
    private static string GetMessageFrom(MethodInfo method)
    {
        return $"""
                Method {
                    method.Name
                }() isn't callable by Destry.Http.

                To fix it you can use one of those attributes above {
                    method.Name
                } of {
                    method.DeclaringType?.Name
                }: [SendGet], [SendHead], [SendPost], [SendPut], [SendPatch], [SendDelete].

                Example of valid {
                    method.DeclaringType?.Name
                }:
                [SendGet]
                {
                    method.ReturnType
                } {
                    method.Name
                }({
                    GetReadableMethodParameter(method)
                })
                """;
    }

    private static string GetReadableMethodParameter(MethodInfo method)
    {
        var stringBuilder = new StringBuilder();

        foreach (var parameter in method.GetParameters())
        {
            stringBuilder.Append(parameter.ParameterType.Name);
            stringBuilder.Append(' ');
            stringBuilder.Append(parameter.Name);
            stringBuilder.Append(',');
        }

        stringBuilder.Remove(stringBuilder.Length - 1, 1);
        return stringBuilder.ToString();
    }
}
