using CsvHelper.Configuration;
using CsvHelper;
using Models;
using Models.Exports;
using Newtonsoft.Json;
using Services;
using System.Globalization;
using System.Text;
using Models.Enums;
using Services.Interfaces;

namespace ExportTool
{
    public class ExportEmployeeService : IExportService<Employee, EmployeeExportModel>
    {
        public string PathToDirectory { get; set; }
        public string FileName { get; set; }
        public ExportEmployeeService(string pathToDirectory, string csvFileName)
        {
            PathToDirectory = pathToDirectory;
            FileName = csvFileName;
        }
        public ExportEmployeeService() { }
        public async Task ExportForCsv(List<Employee> employees)
        {
            var listEmployee = await ConvertatorToExportListModel(employees);
            DirectoryInfo dirInfo = new DirectoryInfo(PathToDirectory);
            if (!dirInfo.Exists) dirInfo.Create();
            string fullPath = Path.Combine(PathToDirectory, FileName);
            using var fileStream = new FileStream(fullPath, FileMode.Create);
            using var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
            await csvWriter.WriteRecordsAsync(listEmployee);
            await csvWriter.FlushAsync();
        }
        public async Task ImportFromCsvToDb()
        {
            string fullPath = Path.Combine(PathToDirectory, FileName);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            using var streamReader = new StreamReader(fullPath);
            using var csvReader = new CsvReader(streamReader, config);
            var employeeService = new EmployeeService();
            var employeeList = new List<Employee>();
            var clientsListFromDb = await employeeService.GetEmployees();
            await csvReader.ReadAsync();
            csvReader.ReadHeader();
            while (await csvReader.ReadAsync())
            {
                var employee = new Employee(
                    csvReader.GetField<Guid>("PassportId"),
                    csvReader.GetField("NumberPhone")!,
                    csvReader.GetField("Name")!,
                    csvReader.GetField<int>("Age"),
                    csvReader.GetField<JobPosition>("JobPosition"),
                    csvReader.GetField<Guid>("Id"),
                    csvReader.GetField<DateTime>("StartDateWork"),
                    csvReader.GetField<DateTime>("EndDateWork"))
                {
                    InBlackList = csvReader.GetField<bool>("InBlackList"),
                    Id = csvReader.GetField<Guid>("Id")
                };
                employeeList.Add(employee);
            }

            foreach (var employeeCsv in employeeList)
                if (!clientsListFromDb.Any(c => c.PassportId == employeeCsv.PassportId))
                    await employeeService.Add(employeeCsv);
        }
        public async Task<string> ExportForJson(List<Employee> employees)
        {
            var listEmployee = await ConvertatorToExportListModel(employees);
            DirectoryInfo dirInfo = new DirectoryInfo(PathToDirectory);
            if (!dirInfo.Exists) dirInfo.Create();
            string fullPath = Path.Combine(PathToDirectory, FileName);
            using var fileStream = new FileStream(fullPath, FileMode.Create);
            using var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            var jsonEmployees = JsonConvert.SerializeObject(listEmployee);
            await streamWriter.WriteLineAsync(jsonEmployees);
            return jsonEmployees;
        }
        public async Task ImportFromJsonToDb(string employeesJson)
        {
            string fullPath = Path.Combine(PathToDirectory, FileName);
            using var streamReader = new StreamReader(fullPath);
            var employeeService = new EmployeeService();
            var employeesListFromDb = await employeeService.GetEmployees();
            var employees = JsonConvert.DeserializeObject<Employee[]>(employeesJson);

            foreach (var employee in employees!)
                if (!employeesListFromDb.Any(c => c.PassportId == employee.PassportId))
                    await employeeService.Add(employee);
        }
        public async Task<EmployeeExportModel> ConvertatorToExportModel(Employee employee)
            => new EmployeeExportModel(
                    employee.Name,
                    employee.Age,
                    employee.NumberPhone,
                    employee.InBlackList,
                    employee.JobPositionType,
                    await new CurrencyService().Get(employee.CurrencyIdSalary),
                    employee.StartWorkDate,
                    employee.EndContractDate,
                    employee.PassportId)
            { Id = employee.Id };
        
        public async Task<List<EmployeeExportModel>> ConvertatorToExportListModel(List<Employee> employees)
        {
            List<EmployeeExportModel> employeeExports = new List<EmployeeExportModel>();
            foreach (var employee in employees)
                employeeExports
                    .Add(await ConvertatorToExportModel(employee));
            return employeeExports;
        }

    }
}
