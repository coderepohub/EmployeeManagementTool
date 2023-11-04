using EmployeeManagementTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementTool.Contracts;

public interface IEmployeeAgent
{
    /// <summary>
    /// Get List of Employees
    /// </summary>
    /// <returns>Returns list of all Empliyees.</returns>
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();

    /// <summary>
    /// Search Employee By Name
    /// </summary>
    /// <param name="name">name of employee.</param>
    /// <returns>Returns list of all employee details matching name.</returns>
    Task<IEnumerable<Employee>> SearchEmployeeByNameAsync(string name);

    /// <summary>
    /// Search Employee By Id
    /// </summary>
    /// <param name="id">Id of the employee.</param>
    /// <returns>Return employee by id.</returns>
    Task<Employee> SearchEmployeeByIdAsync(int id);

    /// <summary>
    /// Save a new Employee.
    /// </summary>
    /// <param name="employee">employee record to add.</param>
    /// <returns>True or False, if record is saved.</returns>
    Task<bool> SaveEmployeeAsync(Employee employee);

    /// <summary>
    /// Edit the employee record.
    /// </summary>
    /// <param name="employee">edited employee record</param>
    /// <returns>True or False, if record is edited.</returns>
    Task<bool> EditEmployeeAsync(Employee employee);

    /// <summary>
    /// Delete the employee record by Id.
    /// </summary>
    /// <param name="id">id of employee to be deleted.</param>
    /// <returns>True or False, if record is deleted.</returns>
    Task<bool> DeleteEmployeeAsync(int id);
}
