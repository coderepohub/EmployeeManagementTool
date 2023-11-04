using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementTool.Models.RestResponses
{
    public class SaveEmployeeResponse
    {
        public bool IsSuccess { get; set; }

        public EmployeeDto Employee { get; set; }

        public IEnumerable<CreateEmployeeErrorResponse> Errors { get; set; }
    }
}
