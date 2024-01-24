using ExportTool;
using Models.Exports;
using Models;
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
            IExportService<Employee, EmployeeExportModel> exportService = new ExportEmployeeService(path, fileCsv);
            EmployeeService employeeService = new EmployeeService();

            //Act
            await exportService.ExportForCsv(await employeeService.GetEmployees());
            using var fileStream = new FileStream(fullPath, FileMode.Open);

            //Assert
            Assert.True(fileStream.Length != 0);
        }
        [Fact]
        public async Task ImportEmployeesFromCsvFileToDbTest()
        {
            //Arrange
            IExportService<Employee, EmployeeExportModel> exportService = new ExportEmployeeService(@"..\..\..\..\ExportTool\ExcelInfo", "employees.csv");
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator generator = new TestDataGenerator();

            //Act
            var employees = await employeeService.GetEmployees();
            var newEmployee = await generator.GenerationEmployee();
            employees.Add(newEmployee);
            await exportService.ExportForCsv(employees);
            await exportService.ImportFromCsvToDb();

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
            IExportService<Employee, EmployeeExportModel> exportService = new ExportEmployeeService(path, fileCsv);
            EmployeeService employeeService = new EmployeeService();

            //Act
            await exportService.ExportForJson(await employeeService.GetEmployees());
            using var fileStream = new FileStream(fullPath, FileMode.Open);

            //Assert
            Assert.True(fileStream.Length != 0);
        }
        [Fact]
        public async Task ImportEmployeesFromJsonFileToDbTest()
        {
            //Arrange
            IExportService<Employee, EmployeeExportModel> exportService = new ExportEmployeeService(@"..\..\..\..\ExportTool\JsonFiles", "employees.json");
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator generator = new TestDataGenerator();

            //Act
            var employees = await employeeService.GetEmployees();
            var newEmployee = await generator.GenerationEmployee();
            employees.Add(newEmployee);
            string jsonEmployees = await exportService.ExportForJson(employees);
            await exportService.ImportFromJsonToDb(jsonEmployees);

            //Assert
            Assert.Contains(await employeeService.GetEmployees(), c => c.PassportId == newEmployee.PassportId);
        }
    }
}
