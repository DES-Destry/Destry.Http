namespace Destry.Http.Parsers;

internal static class ParamParser
{
    public static IEnumerable<Param> ParseParams(this string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        List<Param> @params = [];

        var segments = path.Split('/');

        foreach (var segment in segments)
            if (segment.StartsWith('{') && segment.EndsWith('}'))
            {
                var rawParam = segment.Replace('{', ' ').Replace('}', ' ').Trim();
                var paramParts = rawParam.Split(':');

                if (paramParts.Length == 1)
                    @params.Add(new Param(paramParts[0]));
                if (paramParts.Length == 2)
                    @params.Add(new Param(paramParts[0], paramParts[1]));
            }

        return @params;
    }
}
