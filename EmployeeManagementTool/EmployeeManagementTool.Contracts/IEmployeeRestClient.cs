using EmployeeManagementTool.Models;
using EmployeeManagementTool.Models.RestResponses;

namespace EmployeeManagementTool.Contracts
{
    public interface IEmployeeRestClient
    {
        /// <summary>
        /// Save New Employee Record
        /// </summary>
        /// <param name="employee">Employee Details</param>
        /// <returns>Saved Employee details with Id</returns>
        Task<SaveEmployeeResponse> SaveAsync(EmployeeDto employee);

        /// <summary>
        /// List all Employee
        /// </summary>
        /// <returns>returns all list of employees</returns>
        Task<IEnumerable<EmployeeDto>> GetAllAsync();

        /// <summary>
        /// Search employee by name
        /// </summary>
        /// <param name="name">Enter name to search</param>
        /// <returns>Returns list of employee matches the search name</returns>
        Task<IEnumerable<EmployeeDto>> SearchEmployeeByNameAsync(string name);

        /// <summary>
        /// Search employee by Id
        /// </summary>
        /// <param name="id">Enter id to search</param>
        /// <returns>Returns list of employee matches the Id</returns>
        Task<IEnumerable<EmployeeDto>> SearchEmployeeByIdAsync(int id);

        /// <summary>
        /// Edit the Employee record
        /// </summary>
        /// <param name="Id">Id of the Employee</param>
        /// <param name="employeeDto">Employee Details</param>
        /// <returns>Success or failure</returns>
        Task<bool> EditAsync(int id, EmployeeDto employeeDto);

        /// <summary>
        /// Delete the Employee Record
        /// </summary>
        /// <param name="Id">Endter Employee Id to delete</param>
        /// <returns>returns the deleteion succesfull or not</returns>
        Task<bool> DeleteAsync(int id);
    }
}