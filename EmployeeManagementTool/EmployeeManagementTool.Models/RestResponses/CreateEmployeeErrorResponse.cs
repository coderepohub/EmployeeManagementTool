using Newtonsoft.Json;

namespace EmployeeManagementTool.Models.RestResponses;

public class CreateEmployeeErrorResponse
{
    [JsonProperty("field")]
    public string Field { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }
}
