using System.Reflection;
using Destry.Http.Controller;
using Destry.Http.Converters;
using Destry.Http.Data;
using Destry.Http.Proxies;
using Destry.Http.Senders;

namespace Destry.Http;

/// <summary>
///     Main class, that you surely will use in any case to build HTTP client. <br /> <br />
///     It's a Builder class that can be configured with special methods. <br />
///     And after configuring call <see cref="From{T}" /> method to get ready HTTP client from specified interface.
/// </summary>
public sealed class ControllerBuilder
{
    private string? _baseUrl;
    private Converter _converter = new JsonConverter();
    private HttpSender _httpSender = new HttpClientHttpSender();

    /// <summary>
    ///     Setup base URL. <br /> <br />
    ///     This URL will used as base for all HTTP requests.
    /// </summary>
    /// <param name="url">
    ///     It's a first part of the path of any HTTP request. <br />
    ///     For example: https://api.some-cool-service.com
    /// </param>
    /// <returns>Current instance to be able continue configuring.</returns>
    public ControllerBuilder WithBaseUrl(string url)
    {
        _baseUrl = url;
        return this;
    }

    /// <summary>
    ///     Specify your custom <see cref="Converter" /> implementation. <br />
    ///     Json converter using by default.
    /// </summary>
    /// <param name="converter">Custom <see cref="Converter" /> implementation.</param>
    /// <returns>Current instance to be able continue configuring.</returns>
    public ControllerBuilder WithConverter(Converter converter)
    {
        _converter = converter;
        return this;
    }

    /// <summary>
    ///     Specify your custom <see cref="HttpSender" /> implementation. <br />
    ///     It using httpSender that using <see cref="System.Net.Http.HttpClient" /> to make HTTP requests by default.
    /// </summary>
    /// <param name="httpSender">Custom <see cref="HttpSender" /> implementation.</param>
    /// <returns>Current instance to be able continue configuring.</returns>
    public ControllerBuilder WithSender(HttpSender httpSender)
    {
        _httpSender = httpSender;
        return this;
    }

    /// <summary>
    ///     Get <c>T</c> interface implementation based on his attributes.
    /// </summary>
    /// <typeparam name="T">Interface that describes API service endpoints with attributes.</typeparam>
    /// <returns>Instance of <c>T</c> with implementation based on attributes.</returns>
    /// <exception cref="NullReferenceException">
    ///     Base URL must be specified with <see cref="ControllerAttribute" /> param in
    ///     interface or with <see cref="WithBaseUrl" /> method. Else this method will crashed.
    /// </exception>
    public T From<T>() where T : class
    {
        var baseUrl = _baseUrl;
        var type = typeof(T);

        var controllerAttribute = type.GetCustomAttribute<ControllerAttribute>(false);
        var keyValueDataAttributes = type.GetCustomAttributes<KeyValueDataAttribute>(true);

        if (controllerAttribute?.BaseUrl is not null)
            baseUrl = controllerAttribute.BaseUrl;

        if (baseUrl is null)
            throw new NullReferenceException(nameof(baseUrl));

        var proxy = DispatchProxy.Create<T, ControllerProxy<T>>();
        ((ControllerProxy<T>) (object) proxy).Initialize(baseUrl, _converter, _httpSender,
            keyValueDataAttributes);

        return proxy;
    }
}
