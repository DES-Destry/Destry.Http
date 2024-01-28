using System.Net;

namespace Destry.Http;

/// <summary>
///     Response object, that MUST be returned by <see cref="HttpSender.SendHttpRequestAsync" /> method. <br /><br />
///     This class isn't a response for client code. Purpose of this class is to be processed by any
///     <see cref="Converter" />.
/// </summary>
public class HttpRawResponse
{
    /// <summary>
    ///     If some exception was thrown while <see cref="HttpSender.SendHttpRequestAsync" /> is executing it will be
    ///     <see langword="true" />. <br /> <br />
    ///     In other cases - <see langword="false" />.
    /// </summary>
    public bool IsError { get; set; }

    /// <summary>
    ///     HTTP Status Code of request after <see cref="HttpSender.SendHttpRequestAsync" /> method execution. <br />
    ///     See more on https://developer.mozilla.org/en-US/docs/Web/HTTP/Status
    /// </summary>
    public HttpStatusCode? Status { get; set; }

    /// <summary>
    ///     If <see cref="IsError" /> equals <see langword="false" />, therefore <see cref="HttpSender.SendHttpRequestAsync" />
    ///     without any problem and there will placed data from request response. <br /> <br />
    ///     In case when <see cref="IsError" /> equals <see langword="true" /> it will contain <see langword="null" /> value,
    ///     be careful.
    /// </summary>
    public Stream? Data { get; set; }

    /// <summary>
    ///     If <see cref="IsError" /> equals <see langword="true" />, therefore <see cref="HttpSender.SendHttpRequestAsync" />
    ///     was not successfully and there will placed a thrown exception. <br /> <br />
    ///     In case when <see cref="IsError" /> equals <see langword="false" /> it will contain <see langword="null" /> value,
    ///     be careful.
    /// </summary>
    public Exception? Exception { get; set; }
    // public Dictionary<string, string> Headers { get; set; }
}
