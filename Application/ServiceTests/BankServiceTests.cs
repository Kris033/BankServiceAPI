using Models;
using Services;
using Xunit;

namespace ServiceTests
{
    public class BankServiceTests
    {
        [Fact]
        public void AddPersonInToBlackListTest()
        {
            //Arrange
            BankService service = new BankService();
            TestDataGenerator generator = new TestDataGenerator();
            EmployeeService employeeService = new EmployeeService();

            //Act
            var employee = generator.GenerationEmployees(1).First();
            service.AddToBlackList(employee);
            employeeService.ChangeEmployee(employee);

            //Assert
            Assert.True(service.IsPersonInBlackList(employee));
        }
    }
}
