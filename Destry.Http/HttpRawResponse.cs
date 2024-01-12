using System.Net;

namespace Destry.Http;

public class HttpRawResponse
{
    public HttpStatusCode Status { get; set; }

    public Stream Data { get; set; }
    // public Dictionary<string, string> Headers { get; set; }
}
