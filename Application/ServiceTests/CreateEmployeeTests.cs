using Models;
using Services;
using Xunit;

namespace ServiceTests
{
    public class CreateEmployeeTests
    {
        [Fact]
        public void CreateEmployeeInServicePositiveTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var employee = new Employee(
                dataGenerator.GenerationPassport(),
                "123-4265-243",
                JobPosition.Security,
                new Currency(400, CurrencyType.Dollar),
                new DateOnly(2021, 5, 21),
                new DateOnly(2025, 1, 23));
            employeeService.AddEmployee(employee);
        }
        [Fact]
        public void CreateEmployeeInServiceNegativeTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => 
            {
                //Act
                var employee = new Employee(
                    dataGenerator.GenerationPassport(),
                    "123-4265-243",
                    JobPosition.Security,
                    new Currency(400, CurrencyType.Dollar),
                    new DateOnly(2025, 1, 23),
                    new DateOnly(2021, 5, 21));
                employeeService.AddEmployee(employee);
            });
        }
    }
}
