using Services;
using Services.Storage;
using Models.Filters;
using Xunit;
using Models;

namespace ServiceTests
{
    public class EmployeeStorageTests
    {
        [Fact]
        public void AddEmployeeInStorageTest()
        {
            //Arrange
            IEmployeeStorage employees = new EmployeeStorage();
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var employee = dataGenerator.GenerationEmployees(1).First();
            employees.Add(employee);

            //Assert
            Assert.Contains(employee, employees.DataEmployees);
        }
        [Fact]
        public void UpdateEmployeeInStorageTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IEmployeeStorage employees = new EmployeeStorage(dataGenerator.GenerationEmployees(10));

            //Act
            var employee = employees.DataEmployees.First();
            employee.UpdateSalary(new Currency(1000, CurrencyType.Dollar));
            employees.Update(employee);

            //Assert
            Assert.Contains(employee, employees.DataEmployees);
        }
        [Fact]
        public void DeleteEmployeeInStorageTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();
            IEmployeeStorage employees = new EmployeeStorage(dataGenerator.GenerationEmployees(10));

            //Act
            var employee = employees.DataEmployees.First();
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
            var employees = employeeService.GetEmployees(new GetFilterRequest() { DateBornFrom = new DateOnly(1996, 1, 1) });

            //Assert
            Assert.DoesNotContain(employees, e => e.Passport!.DateBorn < new DateOnly(1995, 12, 31));
        }
    }
}
