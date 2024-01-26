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
    ///     Instance of <see cref="T" /> or default value of <see cref="T" /> if some critical error was catched.
    /// </returns>
    public abstract Task<T?> FromRawResponseToAsync<T>(Task<HttpRawResponse> response);
}
