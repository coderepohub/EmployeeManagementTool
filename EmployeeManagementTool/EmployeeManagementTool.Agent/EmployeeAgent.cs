using AutoMapper;
using EmployeeManagementTool.Contracts;
using EmployeeManagementTool.Models;
using EmployeeManagementTool.Models.RestResponses;

namespace EmployeeManagementTool.Agent
{
    public class EmployeeAgent : IEmployeeAgent
    {
        private readonly IEmployeeRestClient _employeeRestClient;
        private readonly IMapper _mapper;
        public EmployeeAgent(IEmployeeRestClient employeeRestClient, IMapper mapper)
        {
            _employeeRestClient = employeeRestClient ?? throw new ArgumentNullException(nameof(employeeRestClient));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        ///<inheritdoc/>
        public Task<bool> DeleteEmployeeAsync(int id)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public Task<bool> EditEmployeeAsync(Employee employee)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var result = await _employeeRestClient.GetAllAsync();
            if (result == null)
            {
                return new List<EmployeeDto>();
            }
            return result;
        }

        ///<inheritdoc/>
        public async Task<SaveEmployeeAgentResponse> SaveEmployeeAsync(Employee employee)
        {
            if (employee == null) { throw new ArgumentNullException(nameof(employee)); }

            SaveEmployeeAgentResponse saveEmployeeAgentResponse = new SaveEmployeeAgentResponse()
            {
                IsSuccess = false
            };
            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            var saveEmployeeRestResponse = await _employeeRestClient.SaveAsync(employeeDto);
            if (saveEmployeeRestResponse != null)
            {
                if (saveEmployeeRestResponse.IsSuccess)
                {
                    saveEmployeeRestResponse.IsSuccess = true;
                }
                else
                {
                    if (saveEmployeeRestResponse.Errors != null && saveEmployeeRestResponse.Errors.Any())
                    {
                        string failureMessage = string.Empty;
                        foreach (var item in saveEmployeeRestResponse.Errors)
                        {
                            failureMessage += $"{item.Field} {item.Message}\n";
                        }
                        saveEmployeeAgentResponse.ErrorMessage = failureMessage;
                    }
                }
            }

            return saveEmployeeAgentResponse;
        }

        ///<inheritdoc/>
        public async Task<Employee> SearchEmployeeByIdAsync(int id)
        {
            if (id < 0) { throw new ArgumentOutOfRangeException(nameof(id)); }

            var result = await _employeeRestClient.SearchEmployeeByIdAsync(id);
            if (result != null && result.Any())
            {
                var employee = _mapper.Map<IEnumerable<Employee>>(result);
                return employee.First();
            }

            return null;
        }

        ///<inheritdoc/>
        public Task<IEnumerable<Employee>> SearchEmployeeByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}