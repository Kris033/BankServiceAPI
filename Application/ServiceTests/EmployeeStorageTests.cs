using Services;
using Services.Storage;
using Models.Filters;
using Xunit;

namespace ServiceTests
{
    public class EmployeeStorageTests
    {
        [Fact]
        public void AddInStorageTest()
        {
            //Arrange
            EmployeeStorage employees = new EmployeeStorage();
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var employee = dataGenerator.GenerationEmployees(1).First();
            employees.Add(employee);

            //Assert
            Assert.Contains(employee, employees);
        }
        [Fact]
        public void FilterSearchStorageTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();

            //Act
            var employees = employeeService.GetEmployees(new GetFilterRequest() { DateBornFrom = new DateOnly(1996, 1, 1) });

            //Assert
            Assert.DoesNotContain(employees, e => e.Passport!.DateBorn < new DateOnly(1995, 12, 31));
        }
    }
}
