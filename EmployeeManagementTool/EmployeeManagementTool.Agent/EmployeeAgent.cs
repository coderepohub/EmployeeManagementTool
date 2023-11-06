using AutoMapper;
using EmployeeManagementTool.Contracts;
using EmployeeManagementTool.Models;

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
        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            if (id <= 0)
            {
                return false;
            }
            return await _employeeRestClient.DeleteAsync(id);
        }

        ///<inheritdoc/>
        public async Task<bool> EditEmployeeAsync(EmployeeDto employeeDto)
        {
            int id = employeeDto.Id;
            if (id <= 0)
            {
                return false;
            }
            return await _employeeRestClient.EditAsync(id, employeeDto);
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
                    saveEmployeeAgentResponse.IsSuccess = true;
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
        public async Task<EmployeeDto> SearchEmployeeByIdAsync(int id)
        {
            if (id < 0) { throw new ArgumentOutOfRangeException(nameof(id)); }

            var result = await _employeeRestClient.SearchEmployeeByIdAsync(id);
            if (result != null && result.Any())
            {
                return result.First();
            }

            return null;
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<EmployeeDto>> SearchEmployeeByNameAsync(string name)
        {
            var result = await _employeeRestClient.SearchEmployeeByNameAsync(name);
            if (result == null || !result.Any())
            {
                return new List<EmployeeDto>();
            }
            return result;
        }
    }
}