using System.Diagnostics.CodeAnalysis;
using Destry.Http.Parsers;

namespace Destry.Http.Senders;

public sealed class HttpClientSender : Sender
{
    private readonly Dictionary<string, string> _headers = new();
    private readonly Dictionary<string, string> _params = new();
    private readonly Dictionary<string, string> _queries = new();
    private string _baseUrl;
    private object? _body;

    public override void SetBaseUrl(string url) => _baseUrl = url;
    public override void AddQuery(string name, string value) => _queries.Add(name, value);
    public override void AddHeader(string name, string value) => _headers.Add(name, value);
    public override void AddParam(string name, string value) => _params.Add(name, value);
    public override void SetBody(object body) => _body = body;

    public override async Task<T> SendHttpRequestAsync<T>(
        string httpMethod,
        [StringSyntax("Route")] string path)
    {
        var method = HttpMethod.Parse(httpMethod);
        var parametrizedPath = ApplyParams(path);

        throw new NotImplementedException();
    }

    private string ApplyParams(string path)
    {
        var result = path;
        var @params = path.ParseParams();

        foreach (var param in @params)
        {
            var isValueAssigned = _params.TryGetValue(param.Name, out var parsed);
            var value = isValueAssigned ? parsed! : "null";

            result = result.Replace(param.GetRawString(), value);
        }

        return result;
    }
}
