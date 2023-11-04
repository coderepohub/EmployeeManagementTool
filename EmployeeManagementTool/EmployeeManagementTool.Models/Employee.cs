using EmployeeManagementTool.Models.Enums;

namespace EmployeeManagementTool.Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Gender Gender { get; set; }
    public Status Status { get; set; }
}