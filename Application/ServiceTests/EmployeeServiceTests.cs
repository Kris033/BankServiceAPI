using Bogus;
using Models;
using Models.Enums;
using Services;
using Xunit;

namespace ServiceTests
{
    public class EmployeeServiceTests
    {
        
        [Fact]
        public async void CreateEmployeeInServicePositiveTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var passport = dataGenerator.GenerationPassport();
            await passportService.AddPassport(passport);
            var salary = new Models.Currency(400, CurrencyType.Dollar);
            await currencyService.AddCurrency(salary);
            var dateOnlyToday = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            var dateOnlyFuture2Year = dateOnlyToday.AddYears(2);
            var employee = new Employee(
                passport.Id,
                faker.Random.ReplaceNumbers("###-####-###"),
                passport.GetFullName(),
                passport.GetAge(),
                JobPosition.Security,
                salary.Id,
                dateOnlyToday,
                dateOnlyFuture2Year);
            await employeeService.AddEmployee(employee);

            //Assert
            Assert.NotNull(await employeeService.GetEmployee(employee.Id));
        }
        [Fact]
        public async Task CreateEmployeeInServiceNegativeTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => 
            {
                //Act
                var passport = dataGenerator.GenerationPassport();
                await passportService.AddPassport(passport);
                var salary = new Models.Currency(400, CurrencyType.Dollar);
                await currencyService.AddCurrency(salary);
                var dateOnlyToday = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                var dateOnlyFuture2Year = dateOnlyToday.AddYears(2);
                var employee = new Employee(
                    passport.Id,
                    faker.Random.ReplaceNumbers("###-####-###"),
                    passport.GetFullName(),
                    passport.GetAge(),
                    JobPosition.Security,
                    salary.Id,
                    dateOnlyFuture2Year,
                    dateOnlyToday);
                await employeeService.AddEmployee(employee);
            });
        }
        [Fact]
        public async void UpdateEmployeePositiveTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var passport = dataGenerator.GenerationPassport();
            await passportService.AddPassport(passport);
            var salary = new Models.Currency(400, CurrencyType.Dollar);
            await currencyService.AddCurrency(salary);
            var dateOnlyToday = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            var dateOnlyFuture2Year = dateOnlyToday.AddYears(2);
            var randomNumberPhone = faker.Random.ReplaceNumbers("###-####-###");
            var employee = new Employee(
                passport.Id,
                randomNumberPhone,
                passport.GetFullName(),
                passport.GetAge(),
                JobPosition.Security,
                salary.Id,
                dateOnlyToday,
                dateOnlyFuture2Year);
            await employeeService.AddEmployee(employee);
            employee.ChangeNumberPhone(faker.Random.ReplaceNumbers("###-####-###"));
            await employeeService.ChangeEmployee(employee);
            var employeeFromDb = await employeeService.GetEmployee(employee.Id);

            //Assert
            Assert.True(employeeFromDb?.NumberPhone != randomNumberPhone);
        }
        [Fact]
        public async Task UpdateEmployeeNegativeTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                var passport = dataGenerator.GenerationPassport();
                await passportService.AddPassport(passport);
                var salary = new Models.Currency(400, CurrencyType.Dollar);
                await currencyService.AddCurrency(salary);
                var dateOnlyToday = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                var dateOnlyFuture2Year = dateOnlyToday.AddYears(2);
                var employee = new Employee(
                    passport.Id,
                    faker.Random.ReplaceNumbers("###-####-###"),
                    passport.GetFullName(),
                    passport.GetAge(),
                    JobPosition.Security,
                    salary.Id,
                    dateOnlyToday,
                    dateOnlyFuture2Year);
                await employeeService.AddEmployee(employee);
                employee.Id = passport.Id;
                await employeeService.ChangeEmployee(employee);
            });
        }
        [Fact]
        public async void DeleteEmployeePositiveTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var passport = dataGenerator.GenerationPassport();
            await passportService.AddPassport(passport);
            var salary = new Models.Currency(400, CurrencyType.Dollar);
            await currencyService.AddCurrency(salary);
            var dateOnlyToday = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            var dateOnlyFuture2Year = dateOnlyToday.AddYears(2);
            var employee = new Employee(
                passport.Id,
                faker.Random.ReplaceNumbers("###-####-###"),
                passport.GetFullName(),
                passport.GetAge(),
                JobPosition.Security,
                salary.Id,
                dateOnlyToday,
                dateOnlyFuture2Year);
            await employeeService.AddEmployee(employee);
            await employeeService.DeleteEmployee(employee);

            //Assert
            Assert.Null(await employeeService.GetEmployee(employee.Id));
        }
    }
}
