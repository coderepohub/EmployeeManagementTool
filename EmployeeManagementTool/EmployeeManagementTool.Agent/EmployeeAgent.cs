using EmployeeManagementTool.Contracts;
using EmployeeManagementTool.Models;

namespace EmployeeManagementTool.Agent
{
    public class EmployeeAgent : IEmployeeAgent
    {
        private readonly IEmployeeRestClient _employeeRestClient;
        public EmployeeAgent(IEmployeeRestClient employeeRestClient)
        {
            _employeeRestClient = employeeRestClient ?? throw new ArgumentNullException(nameof(employeeRestClient));
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
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            var result = await _employeeRestClient.GetAllAsync();
            if (result == null)
            {
                return new List<Employee>();
            }
            return result;
        }

        ///<inheritdoc/>
        public async Task<bool> SaveEmployeeAsync(Employee employee)
        {
            bool isEmployeeSaved = false;
            if (employee == null) { throw new ArgumentNullException(nameof(employee)); }

            var result = await _employeeRestClient.SaveAsync(employee);
            if (result != null && result.Id > 0)
            {
                isEmployeeSaved = true;
            }
            return isEmployeeSaved;
        }

        ///<inheritdoc/>
        public async Task<Employee> SearchEmployeeByIdAsync(int id)
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
        public Task<IEnumerable<Employee>> SearchEmployeeByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}