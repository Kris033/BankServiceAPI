using CsvHelper.Configuration;
using CsvHelper;
using Models;
using Newtonsoft.Json;
using Services;
using System.Globalization;
using System.Text;
using Models.Enums;

namespace ExportTool
{
    public class ExportEmployeeService
    {
        private string _pathToDirectory { get; set; }
        private string _csvFileName { get; set; }
        public ExportEmployeeService(string pathToDirectory, string csvFileName)
        {
            _pathToDirectory = pathToDirectory;
            _csvFileName = csvFileName;
        }
        public async Task ExportEmployeesForCsv(List<Employee> employees)
        {
            var listEmployee = ConvertatorEmployees(employees);
            DirectoryInfo dirInfo = new DirectoryInfo(_pathToDirectory);
            if (!dirInfo.Exists) dirInfo.Create();
            string fullPath = Path.Combine(_pathToDirectory, _csvFileName);
            using var fileStream = new FileStream(fullPath, FileMode.Create);
            using var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
            await csvWriter.WriteRecordsAsync(listEmployee);
            await csvWriter.FlushAsync();
        }
        public async Task ImportEmployeesFromCsv()
        {
            string fullPath = Path.Combine(_pathToDirectory, _csvFileName);
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
                    ContractId = csvReader.GetField<Guid?>("Id")
                };
                employeeList.Add(employee);
            }
            foreach (var employeeCsv in employeeList)
            {
                if (!clientsListFromDb.Any(c => c.PassportId == employeeCsv.PassportId))
                {
                    await employeeService.AddEmployee(employeeCsv);
                }
            }
        }
        public async Task<string> ExportEmployeesToJson(List<Employee> employees)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(_pathToDirectory);
            if (!dirInfo.Exists) dirInfo.Create();
            string fullPath = Path.Combine(_pathToDirectory, _csvFileName);
            using var fileStream = new FileStream(fullPath, FileMode.Create);
            using var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            var jsonEmployees = JsonConvert.SerializeObject(employees);
            await streamWriter.WriteLineAsync(jsonEmployees);
            return jsonEmployees;
        }
        public async Task ImportClientsFromJson(string employeesJson)
        {
            string fullPath = Path.Combine(_pathToDirectory, _csvFileName);
            using var streamReader = new StreamReader(fullPath);
            var employeeService = new EmployeeService();
            var employeesListFromDb = await employeeService.GetEmployees();
            var clients = JsonConvert.DeserializeObject<Employee[]>(employeesJson);
            foreach (var client in clients)
            {
                if (!employeesListFromDb.Any(c => c.PassportId == client.PassportId))
                {
                    await employeeService.AddEmployee(client);
                }
            }
        }
        private List<EmployeeModelCsv> ConvertatorEmployees(List<Employee> employees)
        {
            CurrencyService currencyService = new CurrencyService();
            return employees.Select(async e => 
                new EmployeeModelCsv(
                    e.Name,
                    e.Age,
                    e.NumberPhone,
                    e.InBlackList,
                    e.JobPositionType,
                    await currencyService.GetCurrency(e.CurrencyIdSalary),
                    e.StartWorkDate,
                    e.EndContractDate,
                    e.ContractId,
                    e.PassportId))
                .Select(employeeResult => employeeResult.Result)
                .ToList();
        }
    }
}
