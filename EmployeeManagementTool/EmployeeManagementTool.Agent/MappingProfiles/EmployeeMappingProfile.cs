using AutoMapper;
using EmployeeManagementTool.Models;
using EmployeeManagementTool.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            CreateMap<IEnumerable<Employee>, IEnumerable<EmployeeDto>>();

            CreateMap<EmployeeDto, EmployeeDto>();
            CreateMap<IEnumerable<EmployeeDto>, IEnumerable<EmployeeDto>>();
        }
    }
}
