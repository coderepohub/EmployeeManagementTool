using AutoMapper;
using EmployeeManagementTool.Agent.MappingProfiles;
using EmployeeManagementTool.Models.Enums;
using EmployeeManagementTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementTool.UnitTest
{
    public class EmployeeMappingProfileTests
    {
        private readonly EmployeeMappingProfile _profile;
        private readonly MapperConfiguration _mapperConfiguration;

        public EmployeeMappingProfileTests()
        {
            _profile = new EmployeeMappingProfile();
            _mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(_profile));
        }

        [Fact]
        public void Map_EmployeeToEmployeeDto_StatusMapped()
        {
            // Arrange
            var mapper = _mapperConfiguration.CreateMapper();

            var employee = new Employee
            {
                Id = 1,
                Name = "John",
                Email = "john.xyz@abc.com"
            };

            // Act
            var employeeDto = mapper.Map<EmployeeDto>(employee);

            // Assert
            Assert.Equal(Status.active.ToString(), employeeDto.Status);
        }

        [Fact]
        public void Map_EmployeeListToEmployeeDtoList_StatusMapped()
        {
            // Arrange
            var mapper = _mapperConfiguration.CreateMapper();

            var employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "John" },
            new Employee { Id = 2, Name = "Alice" }
        };

            // Act
            var employeeDtoList = mapper.Map<IEnumerable<EmployeeDto>>(employees);

            // Assert
            Assert.All(employeeDtoList, dto => Assert.Equal(Status.active.ToString(), dto.Status));
        }
    }
}
