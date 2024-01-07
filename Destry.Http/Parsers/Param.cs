using System.Text;

namespace Destry.Http.Parsers;

internal record Param(string Name, string? Type = null)
{
    public string GetRawString()
    {
        var sb = new StringBuilder();
        sb.Append('{');

        sb.Append(Name);

        if (Type is not null)
            sb.Append($":{Type}");

        sb.Append('}');

        return sb.ToString();
    }
}
