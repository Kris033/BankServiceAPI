using Services;
using Services.Storage;
using Xunit;
using Models;
using Models.Enums;
using Models.Requests;

namespace ServiceTests
{
    public class EmployeeStorageTests
    {
        [Fact]
        public async Task AddEmployeeInStorageTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            IEmployeeStorage employees = new EmployeeStorage();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            GetFilterRequest filterRequest = new GetFilterRequest() { CountItem = 1 };

            //Act
            var listEmployee = await employeeService.GetEmployees(filterRequest);
            var employee = listEmployee.FirstOrDefault() 
                ?? await dataGenerator.GenerationEmployee();
            employees.Add(employee);

            //Assert
            Assert.Contains(employee, employees.DataEmployees);
        }
        [Fact]
        public async Task UpdateEmployeeInStorageTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IEmployeeStorage employees = new EmployeeStorage();
            GetFilterRequest filterRequest = new GetFilterRequest() { CountItem = 1 };

            //Act
            var listEmployee = await employeeService.GetEmployees(filterRequest);
            var employee = listEmployee.FirstOrDefault()
                ?? await dataGenerator.GenerationEmployee();
            employees.Add(employee);
            employee.EndContractDate = employee.EndContractDate.AddYears(1);
            employees.Update(employee);

            //Assert
            Assert.Contains(employee, employees.DataEmployees);
        }
        [Fact]
        public async Task DeleteEmployeeInStorageTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IEmployeeStorage employees = new EmployeeStorage();
            GetFilterRequest filterRequest = new GetFilterRequest() { CountItem = 1 };

            //Act
            var listEmployee = await employeeService.GetEmployees(filterRequest);
            var employee = listEmployee.FirstOrDefault()
                ?? await dataGenerator.GenerationEmployee();
            employees.Add(employee);
            employees.Delete(employee);

            //Assert
            Assert.DoesNotContain(employee, employees.DataEmployees);
        }
    }
}
