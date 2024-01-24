using Models;
using Services;
using Services.Interfaces;
using Xunit;

namespace ServiceTests
{
    public class BankServiceTests
    {
        [Fact]
        public async Task AddPersonInToBlackListTest()
        {
            //Arrange
            BankService bankService = new BankService();
            TestDataGenerator generator = new TestDataGenerator();
            IEmployeeService employeeService = new EmployeeService();

            //Act
            var employee = await generator.GenerationEmployee();
            await bankService.AddToBlackList(employee);
            await employeeService.Update(employee);

            //Assert
            Assert.True(bankService.IsPersonInBlackList(employee));
        }
        [Fact]
        public async Task CalculationSalaryBetweenDirectorsTest()
        {
            //Arrange
            BankService bankService = new BankService();
            TestDataGenerator generator = new TestDataGenerator();
            IEmployeeService employeeService = new EmployeeService();
            ICurrencyService currencyService = new CurrencyService();

            //Act
            var salary = 
                await bankService.CalculationSalaryBetweenDirectors(
                    new Currency(2000000, Models.Enums.CurrencyType.EUR),
                    new Currency(1500000, Models.Enums.CurrencyType.MDL));
            var employees = await employeeService.GetEmployees();

            foreach (var employee in employees.Where(e =>
            e.JobPositionType == Models.Enums.JobPosition.Director))
            {
                var currency = await currencyService.Get(employee.CurrencyIdSalary);
                //Assert
                Assert.Equal(currency!.Value, salary.Value);
            }
        }
    }
}
