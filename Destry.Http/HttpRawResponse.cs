using System.Net;

namespace Destry.Http;

public class HttpRawResponse
{
    public bool IsError { get; set; }
    public HttpStatusCode? Status { get; set; }

    public string? Data { get; set; }

    public Exception? Exception { get; set; }
    // public Dictionary<string, string> Headers { get; set; }
}
