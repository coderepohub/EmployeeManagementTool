using EmployeeManagementTool.Contracts;
using EmployeeManagementTool.Models;
using EmployeeManagementTool.Models.Confifurations;
using EmployeeManagementTool.Models.Enums;
using EmployeeManagementTool.Models.RestResponses;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace EmployeeManagementTool.RestClient
{
    public class EmployeeRestClient : IEmployeeRestClient
    {

        private readonly Uri EmployeeApiBaseUri;
        private readonly UrlOptions _urlOptions;
        private readonly string _employeeOperationUri;
        private readonly IHttpClientProviders _httpClientProviders;
        private readonly string _token;

        public EmployeeRestClient(IOptions<UrlOptions> options, IHttpClientProviders httpClientProviders)
        {
            _urlOptions = options.Value;
            EmployeeApiBaseUri = new Uri(_urlOptions.BaseUrl);
            _employeeOperationUri = _urlOptions.EmployeeOperationUri;
            _token = _urlOptions.Token;
            _httpClientProviders = httpClientProviders;
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            IEnumerable<EmployeeDto>? employees = null;
            var httpResponseMessage = await SendRequest<IEnumerable<Employee>>(Method.GET, _employeeOperationUri);
            if (httpResponseMessage.IsSuccessStatusCode && httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                var httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                if (httpResponseContent == null)
                    return employees!;
                employees = JsonConvert.DeserializeObject<IEnumerable<EmployeeDto>>(httpResponseContent);
            }

            return employees!;
        }

        ///<inheritdoc/>
        public async Task<SaveEmployeeResponse> SaveAsync(EmployeeDto employee)
        {
            EmployeeDto? createdEmployee = null;
            SaveEmployeeResponse saveEmployeeResponse = new SaveEmployeeResponse() { IsSuccess = false,};
            var httpResponseMessage = await SendRequest<EmployeeDto>(Method.POST, _employeeOperationUri, employee);
            if (httpResponseMessage.IsSuccessStatusCode && httpResponseMessage.StatusCode == HttpStatusCode.Created)
            {
                saveEmployeeResponse.IsSuccess = true;
                var httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                createdEmployee = JsonConvert.DeserializeObject<EmployeeDto>(httpResponseContent);
                saveEmployeeResponse.Employee = createdEmployee;
            }
            else
            {
                saveEmployeeResponse.IsSuccess = false;
                var httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                if(httpResponseContent == null) return saveEmployeeResponse;
                saveEmployeeResponse.Errors = JsonConvert.DeserializeObject<IEnumerable<CreateEmployeeErrorResponse>>(httpResponseContent);
            }
            return saveEmployeeResponse!;
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<EmployeeDto>> SearchEmployeeByNameAsync(string name)
        {
            IEnumerable<EmployeeDto>? employees = null;
            var uri = $"{_employeeOperationUri}?name={name}";
            var httpResponseMessage = await SendRequest<IEnumerable<EmployeeDto>>(Method.GET, uri);
            if (httpResponseMessage.IsSuccessStatusCode && httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                var httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                if (httpResponseContent == null)
                    return employees!;
                employees = JsonConvert.DeserializeObject<IEnumerable<EmployeeDto>>(httpResponseContent);
            }

            return employees!;
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<EmployeeDto>> SearchEmployeeByIdAsync(int id)
        {
            IEnumerable<EmployeeDto>? employees = null;
            var uri = $"{_employeeOperationUri}?id={id}";
            var httpResponseMessage = await SendRequest<IEnumerable<EmployeeDto>>(Method.GET, uri);
            if (httpResponseMessage.IsSuccessStatusCode && httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                var httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                if (httpResponseContent == null)
                    return employees!;
                employees = JsonConvert.DeserializeObject<IEnumerable<EmployeeDto>>(httpResponseContent);
            }

            return employees!;
        }

        ///<inheritdoc/>
        public async Task<bool> EditAsync(int id, EmployeeDto employeeDto)
        {
            bool isEdited = false;
            var uri = $"{_employeeOperationUri}/{id}";
            var httpResponseMessage = await SendRequest<EmployeeDto>(Method.PATCH, uri,employeeDto);
            if (httpResponseMessage.IsSuccessStatusCode && httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                isEdited = true;
                return isEdited;
            }

            return isEdited;
        }

        ///<inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            bool isEdited = false;
            var uri = $"{_employeeOperationUri}/{id}";
            var httpResponseMessage = await SendRequest<EmployeeDto>(Method.DELETE, uri);
            if (httpResponseMessage.IsSuccessStatusCode && httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                isEdited = true;
                return isEdited;
            }

            return isEdited;
        }


        #region Private Methods
        private async Task<HttpResponseMessage> SendRequest<T>(Method method, string uri, T body = null) where T : class
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentException($"uri can not be null or empty");
            }

            HttpResponseMessage response = new HttpResponseMessage();

            using (var httpClient = new HttpClient(CreateHandler()) { BaseAddress = EmployeeApiBaseUri })
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                    switch (method)
                    {
                        case Method.GET:
                            response = await _httpClientProviders.GetJsonAsync(httpClient, uri);
                            break;
                        case Method.POST:
                            var serializerSettingsPost = new JsonSerializerSettings();
                            serializerSettingsPost.ContractResolver = new CamelCasePropertyNamesContractResolver();
                            var bodySerializedPost = JsonConvert.SerializeObject(body, serializerSettingsPost);
                            var formattedBodyPost = new StringContent(bodySerializedPost, Encoding.UTF8, "application/json");
                            response = await _httpClientProviders.PostAsync(httpClient, uri, formattedBodyPost);
                            break;
                        case Method.PATCH:
                            var serializerSettings = new JsonSerializerSettings();
                            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                            var bodySerialized = JsonConvert.SerializeObject(body, serializerSettings);
                            var formattedBody = new StringContent(bodySerialized, Encoding.UTF8, "application/json");
                            response = await _httpClientProviders.PatchAsync(httpClient, uri, formattedBody);
                            break;
                        case Method.DELETE:
                            response = await _httpClientProviders.DeleteAsync(httpClient, uri);
                            break;
                        default:
                            response = new HttpResponseMessage()
                            {
                                StatusCode = HttpStatusCode.MethodNotAllowed,
                            };
                            break;
                    }
                }
                catch (Exception ex)
                {

                    response = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        Content = new StringContent(ex.Message)
                    };
                }
                return response;
            }

        }

        private HttpClientHandler CreateHandler()
        {
            var clientHandler = new HttpClientHandler();

            clientHandler.CookieContainer = new CookieContainer();

            return clientHandler;
        }
        #endregion
    }
}