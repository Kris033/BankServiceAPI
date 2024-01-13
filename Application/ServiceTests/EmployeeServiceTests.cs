using Bogus;
using Bogus.DataSets;
using Models;
using Models.Enums;
using Services;
using Xunit;

namespace ServiceTests
{
    public class EmployeeServiceTests
    {
        
        [Fact]
        public void CreateEmployeeInServicePositiveTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var passport = dataGenerator.GenerationPassport();
            passportService.AddPassport(passport);
            var salary = new Models.Currency(400, CurrencyType.Dollar);
            currencyService.AddCurrency(salary);
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
            employeeService.AddEmployee(employee);

            //Assert
            Assert.NotNull(employeeService.GetEmployee(employee.Id));
        }
        [Fact]
        public void CreateEmployeeInServiceNegativeTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => 
            {
                //Act
                var passport = dataGenerator.GenerationPassport();
                passportService.AddPassport(passport);
                var salary = new Models.Currency(400, CurrencyType.Dollar);
                currencyService.AddCurrency(salary);
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
                employeeService.AddEmployee(employee);
            });
        }
        [Fact]
        public void UpdateEmployeePositiveTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var passport = dataGenerator.GenerationPassport();
            passportService.AddPassport(passport);
            var salary = new Models.Currency(400, CurrencyType.Dollar);
            currencyService.AddCurrency(salary);
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
            employeeService.AddEmployee(employee);
            employee.ChangeNumberPhone(faker.Random.ReplaceNumbers("###-####-###"));
            employeeService.ChangeEmployee(employee);

            //Assert
            Assert.True(employeeService.GetEmployee(employee.Id)?.NumberPhone != randomNumberPhone);
        }
        [Fact]
        public void UpdateEmployeeNegativeTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                var passport = dataGenerator.GenerationPassport();
                passportService.AddPassport(passport);
                var salary = new Models.Currency(400, CurrencyType.Dollar);
                currencyService.AddCurrency(salary);
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
                employeeService.AddEmployee(employee);
                employee.Id = passport.Id;
                employeeService.ChangeEmployee(employee);
            });
        }
        [Fact]
        public void DeleteEmployeePositiveTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var passport = dataGenerator.GenerationPassport();
            passportService.AddPassport(passport);
            var salary = new Models.Currency(400, CurrencyType.Dollar);
            currencyService.AddCurrency(salary);
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
            employeeService.AddEmployee(employee);
            employeeService.DeleteEmployee(employee);

            //Assert
            Assert.Null(employeeService.GetEmployee(employee.Id));
        }
    }
}
