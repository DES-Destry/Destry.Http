using System.Net.Http.Json;
using System.Text;
using Destry.Http.Parsers;

namespace Destry.Http.Senders;

internal sealed class HttpClientSender : Sender
{
    private readonly HttpClient _client = new();

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

    public override async Task<HttpRawResponse> SendHttpRequestAsync(
        string httpMethod,
        string resource)
    {
        var method = HttpMethod.Parse(httpMethod);

        var request = BuildRequest(method, resource);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return new HttpRawResponse
        {
            Status = response.StatusCode,
            // Headers = response.Headers,
            Data = await response.Content.ReadAsStringAsync()
        };
    }

    private string ApplyParams(string path)
    {
        if (_params.Count == 0) return path;

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

    private string ApplyQuery(string path)
    {
        if (_queries.Count == 0) return path;

        var stringBuilder = new StringBuilder(path);

        foreach (var (query, i) in _queries.Select((query, i) => (query, i)))
        {
            var separator = i == 0 ? "?" : "&";
            stringBuilder.Append($"{separator}{query.Key}={query.Value}");
        }

        return stringBuilder.ToString();
    }

    private Uri BuildUri(string resource)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(_baseUrl);

        var parametrizedResource = ApplyParams(resource);
        var queriedResource = ApplyQuery(parametrizedResource);

        return new Uri($"{_baseUrl}/{queriedResource}");
    }

    private HttpRequestMessage BuildRequest(HttpMethod method, string resource)
    {
        var uri = BuildUri(resource);

        var message = new HttpRequestMessage
        {
            Method = method,
            RequestUri = uri
        };

        foreach (var header in _headers)
            message.Headers.Add(header.Key, header.Value);

        if (_body != null && method != HttpMethod.Get)
            message.Content = JsonContent.Create(_body);

        return message;
    }
}
