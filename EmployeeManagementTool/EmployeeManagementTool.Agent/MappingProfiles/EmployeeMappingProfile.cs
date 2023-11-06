using AutoMapper;
using EmployeeManagementTool.Models;
using EmployeeManagementTool.Models.Enums;

namespace EmployeeManagementTool.Agent.MappingProfiles
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            CreateMap<Employee, EmployeeDto>().AfterMap((s, d) =>
            {
                d.Status = Enum.GetValues(typeof(Status)).GetValue(0).ToString();
            });

            CreateMap<EmployeeDto, Employee>();
        }
    }
}
