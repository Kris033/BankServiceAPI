using Bogus;
using Models;
using Models.Enums;
using Models.Requests;
using Services;
using Xunit;

namespace ServiceTests
{
    public class EmployeeServiceTests
    {
        
        [Fact]
        public async Task CreateEmployeeInServicePositiveTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var employee = await dataGenerator.GenerationEmployee();

            //Assert
            Assert.NotNull(await employeeService.Get(employee.Id));
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
                var passport = await dataGenerator.GenerationPassport();
                var salary = await dataGenerator.GenerationCurrency();
                var dateTimeToday = DateTime.Today;
                var dateFuture2Year = dateTimeToday.AddYears(2);
                var employee = new Employee(
                    passport.Id,
                    faker.Random.ReplaceNumbers("###-####-###"),
                    passport.GetFullName(),
                    passport.GetAge(),
                    JobPosition.Security,
                    salary.Id,
                    dateFuture2Year,
                    dateTimeToday);
                await employeeService.Add(employee);
            });
        }
        [Fact]
        public async void UpdateEmployeePositiveTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator dataGenerator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var employee = await dataGenerator.GenerationEmployee();
            var oldNumberPhone = employee.NumberPhone;
            employee.ChangeNumberPhone(faker.Random.ReplaceNumbers("###-####-###"));
            await employeeService.Update(employee);
            var employeeFromDb = await employeeService.Get(employee.Id);

            //Assert
            Assert.True(employeeFromDb?.NumberPhone != oldNumberPhone);
        }
        [Fact]
        public async Task UpdateEmployeeNegativeTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                var employee = await dataGenerator.GenerationEmployee();
                employee.Id = employee.PassportId;
                await employeeService.Update(employee);
            });
        }
        [Fact]
        public async void DeleteEmployeePositiveTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var employee = await dataGenerator.GenerationEmployee();
            await employeeService.Delete(employee.Id);

            //Assert
            Assert.Null(await employeeService.Get(employee.Id));
        }
        [Fact]
        public async Task FilterSearchStorageTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();

            //Act
            List<Employee> employees = await employeeService.GetEmployees(new GetFilterRequest() { DateBornFrom = new DateTime(1996, 1, 1) });

            //Assert
            Assert.DoesNotContain(employees, e => e.Age > 28);
        }
    }
}
