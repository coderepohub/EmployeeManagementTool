using EmployeeManagementTool.Models;

namespace EmployeeManagementTool.Contracts
{
    public interface IEmployeeRestClient
    {
        /// <summary>
        /// Save New Employee Record
        /// </summary>
        /// <param name="employee">Employee Details</param>
        /// <returns>Saved Employee details with Id</returns>
        Task<Employee> SaveAsync(Employee employee);

        /// <summary>
        /// List all Employee
        /// </summary>
        /// <returns>returns all list of employees</returns>
        Task<IEnumerable<Employee>> GetAllAsync();

        /// <summary>
        /// Search employee by name
        /// </summary>
        /// <param name="name">Enter name to search</param>
        /// <returns>Returns list of employee matches the search name</returns>
        Task<IEnumerable<Employee>> SearchEmployeeByNameAsync(string name);

        /// <summary>
        /// Search employee by Id
        /// </summary>
        /// <param name="id">Enter id to search</param>
        /// <returns>Returns list of employee matches the Id</returns>
        Task<IEnumerable<Employee>> SearchEmployeeByIdAsync(int id);

        /// <summary>
        /// Edit the Employee record
        /// </summary>
        /// <param name="Id">Id of the Employee</param>
        /// <returns>Success or failure</returns>
        Task<bool> EditAsync(int id);

        /// <summary>
        /// Delete the Employee Record
        /// </summary>
        /// <param name="Id">Endter Employee Id to delete</param>
        /// <returns>returns the deleteion succesfull or not</returns>
        Task<bool> DeleteAsync(int id);
    }
}