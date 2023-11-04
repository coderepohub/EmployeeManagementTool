namespace EmployeeManagementTool.Contracts;

public interface IHttpClientProviders
{
    /// <summary>
    /// Http Get Method call.
    /// </summary>
    /// <param name="httpClient">Http client.</param>
    /// <param name="uri">uri path of the api.</param>
    /// <returns>Http Response Message.</returns>
    Task<HttpResponseMessage> GetJsonAsync(HttpClient httpClient, string uri);

    /// <summary>
    /// Http Patch Method Call.
    /// </summary>
    /// <param name="httpClient">Http Client.</param>
    /// <param name="uri">uri path of the api.</param>
    /// <param name="body">Body to be passed in stringcontent.</param>
    /// <returns>HttpResponse Message.</returns>
    Task<HttpResponseMessage> PatchAsync(HttpClient httpClient, string uri, StringContent body);

    /// <summary>
    /// Http Post Method Call.
    /// </summary>
    /// <param name="httpClient">Http Client.</param>
    /// <param name="uri">uri path of the api.</param>
    /// <param name="body">Body to be passed in stringcontent.</param>
    /// <returns>HttpResponse Message.</returns>
    Task<HttpResponseMessage> PostAsync(HttpClient httpClient, string uri, StringContent body);

    /// <summary>
    /// Http Delete Method Call.
    /// </summary>
    /// <param name="httpClient">Http Client.</param>
    /// <param name="uri">uri path of the api.</param>
    /// <returns>HttpResponse Message.</returns>
    Task<HttpResponseMessage> DeleteAsync(HttpClient httpClient, string uri);
}
