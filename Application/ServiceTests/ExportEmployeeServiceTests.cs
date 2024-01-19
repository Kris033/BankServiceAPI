using Bogus;
using ExportTool;
using Models;
using Models.Enums;
using Services;
using Xunit;

namespace ServiceTests
{
    public class ExportEmployeeServiceTests
    {
        [Fact]
        public async Task ExportEmployeesToCsvFileTest()
        {
            //Arrange
            string path = @"..\..\..\..\ExportTool\ExcelInfo";
            string fileCsv = "employees.csv";
            string fullPath = Path.Combine(path, fileCsv);
            ExportEmployeeService exportService = new ExportEmployeeService(path, fileCsv);
            EmployeeService employeeService = new EmployeeService();

            //Act
            await exportService.ExportEmployeesForCsv(await employeeService.GetEmployees());
            using var fileStream = new FileStream(fullPath, FileMode.Open);

            //Assert
            Assert.True(fileStream.Length != 0);
        }
        [Fact]
        public async Task ImportEmployeesFromCsvFileToDbTest()
        {
            //Arrange
            ExportEmployeeService exportService = new ExportEmployeeService(@"..\..\..\..\ExportTool\ExcelInfo", "employees.csv");
            EmployeeService employeeService = new EmployeeService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator generator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var employees = await employeeService.GetEmployees();
            var passport = generator.GenerationPassport();
            var currency = new Currency(faker.Random.Number(15000), faker.PickRandom<CurrencyType>());
            await currencyService.AddCurrency(currency);
            await passportService.AddPassport(passport);
            Employee newEmployee = new Employee(
                passport.Id,
                faker.Random.ReplaceNumbers("###-####-###"),
                passport.GetFullName(),
                passport.GetAge(),
                faker.PickRandom<JobPosition>(),
                currency.Id,
                faker.Date.BetweenDateOnly(
                    new DateOnly(2003, 10, 5),
                    new DateOnly(2024, 1, 3))
                .ToDateTime(new TimeOnly()),
                faker.Date.FutureDateOnly(3)
                .ToDateTime(new TimeOnly()));
            employees.Add(newEmployee);
            await exportService.ExportEmployeesForCsv(employees);
            await exportService.ImportEmployeesFromCsv();

            //Assert
            Assert.Contains(await employeeService.GetEmployees(), c => c.PassportId == newEmployee.PassportId);
        }
        [Fact]
        public async Task ExportEmployeesToJsonFileTest()
        {
            //Arrange
            string path = @"..\..\..\..\ExportTool\JsonFiles";
            string fileCsv = "employees.json";
            string fullPath = Path.Combine(path, fileCsv);
            ExportEmployeeService exportService = new ExportEmployeeService(path, fileCsv);
            EmployeeService employeeService = new EmployeeService();

            //Act
            await exportService.ExportEmployeesToJson(await employeeService.GetEmployees());
            using var fileStream = new FileStream(fullPath, FileMode.Open);

            //Assert
            Assert.True(fileStream.Length != 0);
        }
        [Fact]
        public async Task ImportEmployeesFromJsonFileToDbTest()
        {
            //Arrange
            ExportEmployeeService exportService = new ExportEmployeeService(@"..\..\..\..\ExportTool\JsonFiles", "employees.json");
            EmployeeService employeeService = new EmployeeService();
            PassportService passportService = new PassportService();
            CurrencyService currencyService = new CurrencyService();
            TestDataGenerator generator = new TestDataGenerator();
            Faker faker = new Faker();

            //Act
            var employees = await employeeService.GetEmployees();
            var passport = generator.GenerationPassport();
            var currency = new Currency(faker.Random.Number(15000), faker.PickRandom<CurrencyType>());
            await currencyService.AddCurrency(currency);
            await passportService.AddPassport(passport);
            Employee newEmployee = new Employee(
                passport.Id,
                faker.Random.ReplaceNumbers("###-####-###"),
                passport.GetFullName(),
                passport.GetAge(),
                faker.PickRandom<JobPosition>(),
                currency.Id,
                faker.Date.BetweenDateOnly(
                    new DateOnly(2003, 10, 5),
                    new DateOnly(2024, 1, 3))
                .ToDateTime(new TimeOnly()),
                faker.Date.FutureDateOnly(3)
                .ToDateTime(new TimeOnly()));
            employees.Add(newEmployee);
            string jsonEmployees = await exportService.ExportEmployeesToJson(employees);
            await exportService.ImportClientsFromJson(jsonEmployees);

            //Assert
            Assert.Contains(await employeeService.GetEmployees(), c => c.PassportId == newEmployee.PassportId);
        }
    }
}
