using AutoMapper;
using EmployeeManagementTool.Agent;
using EmployeeManagementTool.Contracts;
using EmployeeManagementTool.Models;
using EmployeeManagementTool.Models.RestResponses;
using Moq;

namespace EmployeeManagementTool.UnitTest
{
    public class EmployeeManagementToolAgentTests
    {
        private readonly Mock<IEmployeeRestClient> _mockEmployeeRestClient;
        private readonly IEmployeeAgent _employeeAgent;
        private readonly Mock<IMapper> _mapperMock;

        public EmployeeManagementToolAgentTests()
        {
            _mockEmployeeRestClient = new Mock<IEmployeeRestClient>();
            _mapperMock = new Mock<IMapper>();
            _employeeAgent = new EmployeeAgent(_mockEmployeeRestClient.Object, _mapperMock.Object);
        }

        #region GetAllEmployee Test Cases
        [Fact]
        public async Task GetAllEmployeesAsync_WithNonNullResult_ReturnsResult()
        {
            // Arrange
            var expectedResult = new List<EmployeeDto>
        {
            new EmployeeDto { Id = 1, Name = "John" },
            new EmployeeDto { Id = 2, Name = "Alice" }
        };

            _mockEmployeeRestClient.Setup(client => client.GetAllAsync()).ReturnsAsync(expectedResult);

            // Act
            var result = await _employeeAgent.GetAllEmployeesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task GetAllEmployeesAsync_WithNullResult_ReturnsEmptyList()
        {
            // Arrange
            _mockEmployeeRestClient.Setup(client => client.GetAllAsync()).ReturnsAsync((List<EmployeeDto>)null);

            // Act
            var result = await _employeeAgent.GetAllEmployeesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
        #endregion

        #region SaveEmployee Test Cases
        [Fact]
        public async Task SaveEmployeeAsync_WithValidEmployee_ReturnsSuccessResponse()
        {
            // Arrange
            var validEmployee = new Employee { Id = 1, Name = "John" };
            var employeeDto = new EmployeeDto { Id = 1, Name = "John" };

            var saveEmployeeRestResponse = new SaveEmployeeResponse
            {
                IsSuccess = true
            };

            _mockEmployeeRestClient.Setup(client => client.SaveAsync(employeeDto)).ReturnsAsync(saveEmployeeRestResponse);

            _mapperMock.Setup(m => m.Map<EmployeeDto>(validEmployee))
      .Returns(employeeDto);

            // Act
            var result = await _employeeAgent.SaveEmployeeAsync(validEmployee);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public async Task SaveEmployeeAsync_WithValidEmployeeButUnsuccessfulResponse_ReturnsFailureResponse()
        {
            // Arrange
            var validEmployee = new Employee { Id = 1, Name = "John" };
            var employeeDto = new EmployeeDto { Id = 1, Name = "John" };

            var saveEmployeeRestResponse = new SaveEmployeeResponse
            {
                IsSuccess = false
            };

            _mockEmployeeRestClient.Setup(client => client.SaveAsync(employeeDto)).ReturnsAsync(saveEmployeeRestResponse);
            _mapperMock.Setup(m => m.Map<EmployeeDto>(validEmployee))
      .Returns(employeeDto);
            // Act
            var result = await _employeeAgent.SaveEmployeeAsync(validEmployee);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public async Task SaveEmployeeAsync_WithNullEmployee_ThrowsArgumentNullException()
        {
            // Arrange
            Employee nullEmployee = null;

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _employeeAgent.SaveEmployeeAsync(nullEmployee));
        }

        [Fact]
        public async Task SaveEmployeeAsync_WithValidationErrors_ReturnsFailureResponseWithErrorMessage()
        {
            // Arrange
            var validEmployee = new Employee { Id = 1, Name = "John" };
            var employeeDto = new EmployeeDto { Id = 1, Name = "John" };

            var saveEmployeeRestResponse = new SaveEmployeeResponse
            {
                IsSuccess = false,
                Errors = new List<CreateEmployeeErrorResponse>
            {
                new CreateEmployeeErrorResponse { Field = "Name", Message = "Invalid Name" }
            }
            };

            _mockEmployeeRestClient.Setup(client => client.SaveAsync(employeeDto)).ReturnsAsync(saveEmployeeRestResponse);
            _mapperMock.Setup(m => m.Map<EmployeeDto>(validEmployee))
     .Returns(employeeDto);

            // Act
            var result = await _employeeAgent.SaveEmployeeAsync(validEmployee);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.ErrorMessage);
            Assert.Contains("Name Invalid Name", result.ErrorMessage);
        }
        #endregion

        #region SearchEmployeeByName Test cases
        [Fact]
        public async Task SearchEmployeeByNameAsync_WithValidNameAndResults_ReturnsEmployeeDtos()
        {
            // Arrange
            string validName = "John";
            var expectedResult = new List<EmployeeDto>
        {
            new EmployeeDto { Id = 1, Name = "John" },
            new EmployeeDto { Id = 2, Name = "John" }
        };

            _mockEmployeeRestClient.Setup(client => client.SearchEmployeeByNameAsync(validName)).ReturnsAsync(expectedResult);

            // Act
            var result = await _employeeAgent.SearchEmployeeByNameAsync(validName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task SearchEmployeeByNameAsync_WithValidNameAndNoResults_ReturnsEmptyList()
        {
            // Arrange
            string validName = "John";
            _mockEmployeeRestClient.Setup(client => client.SearchEmployeeByNameAsync(validName)).ReturnsAsync(new List<EmployeeDto>());

            // Act
            var result = await _employeeAgent.SearchEmployeeByNameAsync(validName);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchEmployeeByNameAsync_WithNullOrEmptyName_ReturnsEmptyList()
        {
            // Arrange
            string emptyName = string.Empty;

            // Act
            var result = await _employeeAgent.SearchEmployeeByNameAsync(emptyName);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchEmployeeByNameAsync_WithNullName_ReturnsEmptyList()
        {
            // Arrange
            string nullName = null;

            // Act
            var result = await _employeeAgent.SearchEmployeeByNameAsync(nullName);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
        #endregion

        #region SearchEmployeeById Test Cases
        [Fact]
        public async Task SearchEmployeeByIdAsync_WithValidIdAndResults_ReturnsEmployeeDto()
        {
            // Arrange
            int validId = 1;
            var expectedResult = new List<EmployeeDto>
        {
            new EmployeeDto { Id = 1, Name = "John" },
            new EmployeeDto { Id = 2, Name = "Alice" }
        };

            _mockEmployeeRestClient.Setup(client => client.SearchEmployeeByIdAsync(validId)).ReturnsAsync(expectedResult);

            // Act
            var result = await _employeeAgent.SearchEmployeeByIdAsync(validId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult[0], result);
        }

        [Fact]
        public async Task SearchEmployeeByIdAsync_WithValidIdAndNoResults_ReturnsNull()
        {
            // Arrange
            int validId = 1;

            _mockEmployeeRestClient.Setup(client => client.SearchEmployeeByIdAsync(validId)).ReturnsAsync(new List<EmployeeDto>());

            // Act
            var result = await _employeeAgent.SearchEmployeeByIdAsync(validId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SearchEmployeeByIdAsync_WithNegativeId_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            int negativeId = -1;

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _employeeAgent.SearchEmployeeByIdAsync(negativeId));
        }
        #endregion

        #region DeleteEmployee Test Cases
        [Fact]
        public async Task DeleteEmployeeAsync_WithValidId_ReturnsTrue()
        {
            // Arrange
            int validId = 1;

            _mockEmployeeRestClient.Setup(client => client.DeleteAsync(validId)).ReturnsAsync(true);

            // Act
            bool result = await _employeeAgent.DeleteEmployeeAsync(validId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_WithInvalidId_ReturnsFalse()
        {
            // Arrange
            int invalidId = 0;

            // Act
            bool result = await _employeeAgent.DeleteEmployeeAsync(invalidId);

            // Assert
            Assert.False(result);
        }
        #endregion

        #region EditEmployee Test Cases
        [Fact]
        public async Task EditEmployeeAsync_WithValidEmployeeDto_ReturnsTrue()
        {
            // Arrange
            var validEmployeeDto = new EmployeeDto { Id = 1 };

            _mockEmployeeRestClient.Setup(client => client.EditAsync(validEmployeeDto.Id, validEmployeeDto)).ReturnsAsync(true);

            // Act
            bool result = await _employeeAgent.EditEmployeeAsync(validEmployeeDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task EditEmployeeAsync_WithInvalidEmployeeDto_ReturnsFalse()
        {
            // Arrange
            var invalidEmployeeDto = new EmployeeDto { Id = 0 };

            // Act
            bool result = await _employeeAgent.EditEmployeeAsync(invalidEmployeeDto);

            // Assert
            Assert.False(result);
        }
        #endregion

    }
}
