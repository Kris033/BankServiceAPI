using Services;
using Xunit;

namespace ServiceTests
{
    public class UpdateEmployeeTests
    {
        [Fact]
        public void UpdateEmployeePositiveTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();

            //Act
            var employee = employeeService.GetFirstEmployee();
            employee.Salary.ChangeValue(120);
            employeeService.ChangeEmployee(0, employee);
        }
        [Fact]
        public void UpdateEmployeeNegativeTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                var employee = employeeService.GetFirstEmployee();
                employee.Salary.ChangeValue(120);
                var newEmployee = dataGenerator.GenerationEmployees(1).First();
                employeeService.ChangeEmployee(0, newEmployee);
            });
        }
    }
}
