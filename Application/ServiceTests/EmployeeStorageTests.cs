using Services;
using Services.Storage;
using Models.Filters;
using Xunit;
using Models;
using Models.Enums;

namespace ServiceTests
{
    public class EmployeeStorageTests
    {
        [Fact]
        public void AddEmployeeInStorageTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            IEmployeeStorage employees = new EmployeeStorage();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            GetFilterRequest filterRequest = new GetFilterRequest() { CountItem = 1 };

            //Act
            var employee = employeeService.GetEmployees(filterRequest).FirstOrDefault();
            if (employee == null)
                employee = dataGenerator.GenerationEmployees(1).First();
            employees.Add(employee);

            //Assert
            Assert.Contains(employee, employees.DataEmployees);
        }
        [Fact]
        public void UpdateEmployeeInStorageTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IEmployeeStorage employees = new EmployeeStorage();
            GetFilterRequest filterRequest = new GetFilterRequest() { CountItem = 1 };

            //Act
            var employee = employeeService.GetEmployees(filterRequest).FirstOrDefault();
            if (employee == null)
                employee = dataGenerator.GenerationEmployees(1).First();
            employees.Add(employee);
            employee.EndContractDate = employee.EndContractDate.AddYears(1);
            employees.Update(employee);

            //Assert
            Assert.Contains(employee, employees.DataEmployees);
        }
        [Fact]
        public void DeleteEmployeeInStorageTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IEmployeeStorage employees = new EmployeeStorage();
            GetFilterRequest filterRequest = new GetFilterRequest() { CountItem = 1 };

            //Act
            var employee = employeeService.GetEmployees(filterRequest).FirstOrDefault();
            if (employee == null)
                employee = dataGenerator.GenerationEmployees(1).First();
            employees.Add(employee);
            employees.Delete(employee);

            //Assert
            Assert.DoesNotContain(employee, employees.DataEmployees);
        }
        [Fact]
        public void FilterSearchStorageTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();

            //Act
            List<Employee> employees = employeeService.GetEmployees(new GetFilterRequest() { DateBornFrom = new DateOnly(1996, 1, 1) });

            //Assert
            Assert.DoesNotContain(employees, e => e.Age > 28);
        }
    }
}
