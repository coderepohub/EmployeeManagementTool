using EmployeeManagementTool.Contracts;
using EmployeeManagementTool.Models;
using EmployeeManagementTool.Models.Confifurations;
using EmployeeManagementTool.Models.RestResponses;
using EmployeeManagementTool.RestClient;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System.Net;

namespace EmployeeManagementTool.UnitTest
{
    public class EmployeeManagementToolRestClientTests
    {
        private readonly Mock<IOptions<UrlOptions>> _mockOptions;
        private readonly Mock<IHttpClientProviders> _httpClientProvidersMock;
        private readonly Mock<HttpClient> _httpClientMock;
        private readonly EmployeeRestClient _employeeRestClient;

        public EmployeeManagementToolRestClientTests()
        {
            _mockOptions = new Mock<IOptions<UrlOptions>>();
            var urlOptions = new UrlOptions
            {
                BaseUrl = "http://example.com",
                EmployeeOperationUri = "employees",
                Token = "token"
            };
            _mockOptions.Setup(o => o.Value).Returns(urlOptions);
            _httpClientProvidersMock = new Mock<IHttpClientProviders>();
            _httpClientMock = new Mock<HttpClient>();
            _employeeRestClient = new EmployeeRestClient(_mockOptions.Object, _httpClientProvidersMock.Object);
        }

        #region GetAllEmployee Test cases
        [Fact]
        public async Task GetAllAsync_ReturnsEmployees_Success()
        {
            // Arrange
            var expectedEmployees = new List<EmployeeDto>
        {
            new EmployeeDto { Id = 1, Name = "Mark" },
            new EmployeeDto { Id = 2, Name = "John" }
        };

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedEmployees))
            };

            _httpClientProvidersMock.Setup(h => h.GetJsonAsync(It.IsAny<HttpClient>(), "employees"))
                .ReturnsAsync(httpResponseMessage);

            // Act
            var employees = await _employeeRestClient.GetAllAsync();

            // Assert
            Assert.NotNull(employees);
            Assert.Equal(expectedEmployees.Count, employees.Count());
            Assert.Equal(expectedEmployees.First().Id, employees.First().Id);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsNullOnFailure()
        {
            // Arrange

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Internal Server Error")
            };

            _httpClientProvidersMock.Setup(h => h.GetJsonAsync(It.IsAny<HttpClient>(), "employees"))
                .ReturnsAsync(httpResponseMessage);

            // Act
            var employees = await _employeeRestClient.GetAllAsync();

            // Assert
            Assert.Null(employees);
        }

        #endregion

        #region SaveEmployee Test Cases
        [Fact]
        public async Task SaveAsync_ReturnsSaveEmployeeResponse_Success()
        {
            // Arrange
            var employeeToSave = new EmployeeDto { Name = "John" };
            var expectedResponse = new SaveEmployeeResponse
            {
                IsSuccess = true,
                Employee = new EmployeeDto { Id = 1, Name = "John" }
            };

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponse))
            };

            _httpClientProvidersMock.Setup(h => h.PostAsync(It.IsAny<HttpClient>(), "employees", It.IsAny<StringContent>()))
                .ReturnsAsync(httpResponseMessage);

            // Act
            var response = await _employeeRestClient.SaveAsync(employeeToSave);

            // Assert
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Employee);
        }


        [Fact]
        public async Task SaveAsync_ReturnsErrorsOnUnprocessableEntity()
        {
            // Arrange
            var employeeToSave = new EmployeeDto { Name = "John", Email="abc@abc.com" };
            var errorContent = new List<CreateEmployeeErrorResponse>
        {
            new CreateEmployeeErrorResponse
            {
                Field = "email",
                Message = "has already been taken"
            }
        };

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Content = new StringContent(JsonConvert.SerializeObject(errorContent))
            };


            _httpClientProvidersMock.Setup(h => h.PostAsync(It.IsAny<HttpClient>(), "employees", It.IsAny<StringContent>()))
                .ReturnsAsync(httpResponseMessage);

            // Act
            var response = await _employeeRestClient.SaveAsync(employeeToSave);

            // Assert
            Assert.False(response.IsSuccess);
            Assert.Null(response.Employee);
            Assert.Equal(response.Errors.Count(), errorContent.Count);
        }


        #endregion


        #region SearchEmployeeByName Test cases
        [Fact]
        public async Task SearchEmployeeByNameAsync_ReturnsEmployeesOnSuccess()
        {
            // Arrange
            var name = "John";
            var expectedEmployees = new List<EmployeeDto>
        {
            new EmployeeDto { Id = 1, Name = "John V D" },
            new EmployeeDto { Id = 2, Name = "John" }
        };

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedEmployees))
            };

            _httpClientProvidersMock.Setup(h => h.GetJsonAsync(It.IsAny<HttpClient>(), "employees?name=John"))
                .ReturnsAsync(httpResponseMessage);

            // Act
            var employees = await _employeeRestClient.SearchEmployeeByNameAsync(name);

            // Assert
            Assert.NotNull(employees);
            Assert.Equal(expectedEmployees.Count, employees.Count());
        }

        [Fact]
        public async Task SearchEmployeeByNameAsync_ReturnsNullOnFailure()
        {
            // Arrange
            var name = "John";

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Internal Server Error")
            };

            _httpClientProvidersMock.Setup(h => h.GetJsonAsync(It.IsAny<HttpClient>(), "employees?name=John"))
                .ReturnsAsync(httpResponseMessage);

            // Act
            var employees = await _employeeRestClient.SearchEmployeeByNameAsync(name);

            // Assert
            Assert.Null(employees);
        }
        #endregion


        #region SearchEmployeeById Test cases
        [Fact]
        public async Task SearchEmployeeByIdAsync_ReturnsEmployeesOnSuccess()
        {
            // Arrange
            int searchId = 1;
            var expectedEmployees = new List<EmployeeDto>
        {
            new EmployeeDto { Id = 1, Name = "John V D" }
        };

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedEmployees))
            };

            _httpClientProvidersMock.Setup(h => h.GetJsonAsync(It.IsAny<HttpClient>(), "employees?id=1"))
                .ReturnsAsync(httpResponseMessage);

            // Act
            var employees = await _employeeRestClient.SearchEmployeeByIdAsync(searchId);

            // Assert
            Assert.NotNull(employees);
            Assert.Equal(expectedEmployees.Count, employees.Count());
        }

        [Fact]
        public async Task SearchEmployeeByIdAsync_ReturnsNullOnFailure()
        {
            // Arrange
            int searchId = 1;

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Internal Server Error")
            };

            _httpClientProvidersMock.Setup(h => h.GetJsonAsync(It.IsAny<HttpClient>(), "employees?id=1"))
                .ReturnsAsync(httpResponseMessage);

            // Act
            var employees = await _employeeRestClient.SearchEmployeeByIdAsync(searchId);

            // Assert
            Assert.Null(employees);
        }
        #endregion


        #region EditEmployee Test Cases
        [Fact]
        public async Task EditAsync_ReturnsTrueOnSuccess()
        {
            // Arrange
            var id = 1;
            var employeeDto = new EmployeeDto { Id = 1, Name = "John" };

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };

            _httpClientProvidersMock.Setup(h => h.PatchAsync(It.IsAny<HttpClient>(), "employees/1", It.IsAny<StringContent>()))
                .ReturnsAsync(httpResponseMessage);

            // Act
            var isEdited = await _employeeRestClient.EditAsync(id, employeeDto);

            // Assert
            Assert.True(isEdited);
        }

        [Fact]
        public async Task EditAsync_ReturnsFalseOnFailure()
        {
            // Arrange
            var id = 1;
            var employeeDto = new EmployeeDto { Id = 1, Name = "John" };

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Internal Server Error")
            };

            _httpClientProvidersMock.Setup(h => h.PatchAsync(It.IsAny<HttpClient>(), "employees/1", It.IsAny<StringContent>()))
                .ReturnsAsync(httpResponseMessage);

            // Act
            var isEdited = await _employeeRestClient.EditAsync(id, employeeDto);

            // Assert
            Assert.False(isEdited);
        }
        #endregion

        #region DeleteEmployee Test Cases
        [Fact]
        public async Task DeleteAsync_ReturnsTrueOnSuccess()
        {
            // Arrange
            var id = 1;

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent
            };

            _httpClientProvidersMock.Setup(h => h.DeleteAsync(It.IsAny<HttpClient>(), "employees/1"))
                .ReturnsAsync(httpResponseMessage);

            // Act
            var isDeleted = await _employeeRestClient.DeleteAsync(id);

            // Assert
            Assert.True(isDeleted);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalseOnFailure()
        {
            // Arrange
            var id = 1;
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Internal Server Error")
            };

            _httpClientProvidersMock.Setup(h => h.DeleteAsync(It.IsAny<HttpClient>(), "employees/1"))
                .ReturnsAsync(httpResponseMessage);

            // Act
            var isDeleted = await _employeeRestClient.DeleteAsync(id);

            // Assert
            Assert.False(isDeleted);
        }

        #endregion
    }
}