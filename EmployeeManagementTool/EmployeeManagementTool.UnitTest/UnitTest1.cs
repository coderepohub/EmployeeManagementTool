using EmployeeManagementTool.Agent;
using EmployeeManagementTool.Models.Confifurations;
using EmployeeManagementTool.RestClient;
using EmployeeManagementTool.RestClient.Helpers;
using Microsoft.Extensions.Options;

namespace EmployeeManagementTool.UnitTest
{
    public class UnitTest1
    {
        private readonly IOptions<UrlOptions> _mockOptions;

        public UnitTest1()
        {
            _mockOptions = Options.Create<UrlOptions>(new UrlOptions()
            {
                BaseUrl = "https://gorest.co.in",
                EmployeeOperationUri = "/public/v2/users",
                Token = "0bf7fb56e6a27cbcadc402fc2fce8e3aa9ac2b40d4190698eb4e8df9284e2023",
            });
        }

        [Fact]
        public async Task Test1()
        {
            var httpClientProvider = new HttpClientProviders();
            var employeeRestCLient = new EmployeeAgent(new EmployeeRestClient(_mockOptions, httpClientProvider));
            var getAllEmployees = await employeeRestCLient.GetAllEmployeesAsync();
        }
    }
}