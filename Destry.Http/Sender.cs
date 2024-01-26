using Destry.Http.Controller;
using Destry.Http.Data;

namespace Destry.Http;

/// <summary>
///     <see cref="Sender" /> is abstract class for building code, that will make a HTTP requests. You can use any HTTP
///     client which you want in <see cref="SendHttpRequestAsync" />. <br /> <br />
///     Don't forget to manage user input with other methods such as <see cref="SetBody" />, <see cref="AddHeader" /> and
///     etc...
/// </summary>
public abstract class Sender
{
    internal Sender NewInstance()
    {
        var type = GetType();
        return (Sender) Activator.CreateInstance(type)!;
    }

    /// <summary>
    ///     You should to save Base URL that was specified with <see cref="ControllerAttribute" /> or
    ///     <see cref="ControllerBuilder.WithBaseUrl" />.
    /// </summary>
    /// <param name="url">Specified URL.</param>
    public abstract void SetBaseUrl(string url);

    /// <summary>
    ///     You should to save specified query parameters from <see cref="QueryAttribute" />.
    /// </summary>
    /// <param name="name">
    ///     Key of the query, specified in param of <see cref="QueryAttribute" /> or extracted from field name.
    /// </param>
    /// <param name="value">Value in the field.</param>
    public abstract void AddQuery(string name, string value);

    /// <summary>
    ///     You should to save specified header from <see cref="HeaderAttribute" />.
    /// </summary>
    /// <param name="name">
    ///     Key of the header, specified in param of <see cref="HeaderAttribute" /> or extracted from field name.
    /// </param>
    /// <param name="value">Value in the field.</param>
    public abstract void AddHeader(string name, string value);

    /// <summary>
    ///     You should to save specified path parameter from <see cref="ParamAttribute" />.
    /// </summary>
    /// <param name="name">
    ///     Key of the path parameter, specified in param of <see cref="ParamAttribute" /> or extracted from field name.
    /// </param>
    /// <param name="value">Value in the field.</param>
    public abstract void AddParam(string name, string value);

    /// <summary>
    ///     You should to save specified request body from <see cref="BodyAttribute" />.
    /// </summary>
    /// <param name="body">
    ///     Data that marked with <see cref="BodyAttribute" />.
    /// </param>
    public abstract void SetBody(object body);

    /// <summary>
    ///     Make a HTTP request as you want.
    /// </summary>
    /// <param name="httpMethod">
    ///     Http method such as GET, POST, etc...
    /// </param>
    /// <param name="path">
    ///     REST resource. For example - posts/{id}.
    /// </param>
    /// <returns>Raw HTTP response. Instance of <see cref="HttpRawResponse" />.</returns>
    public abstract Task<HttpRawResponse> SendHttpRequestAsync(string httpMethod, string path);
}
