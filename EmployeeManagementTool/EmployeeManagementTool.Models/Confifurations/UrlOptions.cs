namespace EmployeeManagementTool.Models.Confifurations;

public class UrlOptions
{
    public const string Name = "EmployeeApiEndpoints";
    public string BaseUrl { get; set; }
    public string EmployeeOperationUri { get; set; }
    public string Token { get; set; }
}
