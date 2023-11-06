namespace EmployeeManagementTool.Models.RestResponses
{
    public class SaveEmployeeResponse
    {
        public bool IsSuccess { get; set; }

        public EmployeeDto Employee { get; set; }

        public IEnumerable<CreateEmployeeErrorResponse> Errors { get; set; }
    }
}
