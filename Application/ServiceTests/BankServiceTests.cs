using Models;
using Services;
using Xunit;

namespace ServiceTests
{
    public class BankServiceTests
    {
        [Fact]
        public async Task AddPersonInToBlackListTest()
        {
            //Arrange
            BankService service = new BankService();
            TestDataGenerator generator = new TestDataGenerator();
            EmployeeService employeeService = new EmployeeService();

            //Act
            var employees = await generator.GenerationEmployees(1);
            var employee = employees.First();
            await service.AddToBlackList(employee);
            await employeeService.ChangeEmployee(employee);

            //Assert
            Assert.True(service.IsPersonInBlackList(employee));
        }
    }
}
