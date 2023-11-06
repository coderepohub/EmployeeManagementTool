using EmployeeManagementTool.Contracts;

namespace EmployeeManagementTool.RestClient.Helpers
{
    public class HttpClientProviders : IHttpClientProviders
    {

        ///<inheritdoc/>
        public async Task<HttpResponseMessage> GetJsonAsync(HttpClient httpClient, string uri)
        {
            return await httpClient.GetAsync(uri);
        }

        ///<inheritdoc/>
        public async Task<HttpResponseMessage> PatchAsync(HttpClient httpClient, string uri, StringContent body)
        {
            return await httpClient.PatchAsync(uri, body);
        }

        ///<inheritdoc/>
        public async Task<HttpResponseMessage> PostAsync(HttpClient httpClient, string uri, StringContent body)
        {
            return await httpClient.PostAsync(uri, body);
        }

        ///<inheritdoc/>
        public async Task<HttpResponseMessage> DeleteAsync(HttpClient httpClient, string uri)
        {
            return await httpClient.DeleteAsync(uri);
        }
    }
}
