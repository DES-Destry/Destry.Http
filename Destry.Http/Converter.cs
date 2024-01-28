namespace Destry.Http;

/// <summary>
///     Converts <see cref="HttpSender" />'s result of work to any response object what you want.
/// </summary>
public abstract class Converter
{
    /// <summary>
    ///     Convert raw http response to data what you want.
    /// </summary>
    /// <param name="response">
    ///     Raw HTTP response with Data or Exception.
    /// </param>
    /// <typeparam name="T">
    ///     The type, how Data's content will represented in method.
    /// </typeparam>
    /// <returns>
    ///     Instance of <c>T</c> or default value of <c>T</c> if some critical error was caught.
    /// </returns>
    public abstract Task<T?> FromRawResponseToAsync<T>(Task<HttpRawResponse> response);
}
