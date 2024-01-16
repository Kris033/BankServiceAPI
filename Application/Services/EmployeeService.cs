using Models;
using Services.Storage;
using Models.Validations;
using BankDbConnection;
using Models.Requests;

namespace Services
{
    public class EmployeeService
    {
        private readonly IEmployeeStorage _employees = new EmployeeStorage();
        private BankContext _bankContext;
        public EmployeeService() 
        {
            _bankContext = new BankContext();
        }
        public Employee GetFirstEmployee() => _employees.DataEmployees.First();
        public List<Employee> GetEmployees(GetFilterRequest? filterRequest = null)
        {
            var employees = _bankContext.Employee.AsQueryable();
            if (filterRequest != null)
            {
                var passportService = new PassportService();
                List<Passport> passportList = new List<Passport>();
                employees.ToList().ForEach(c =>
                {
                    var passport = passportService.GetPassport(c.PassportId);
                    if (passport != null) passportList.Add(passport);
                });
                var passports = passportList.AsQueryable();
                if (!string.IsNullOrWhiteSpace(filterRequest.SearchFullName))
                    employees = employees
                        .Where(e => e
                            .Name
                            .Contains(filterRequest.SearchFullName));
                if (!string.IsNullOrWhiteSpace(filterRequest.SearchNumberPhone))
                    employees = employees
                        .Where(e => e
                            .NumberPhone
                            .Contains(filterRequest.SearchNumberPhone));
                if (!string.IsNullOrWhiteSpace(filterRequest.SearchNumberPassport))
                    passports = passports
                        .Where(p => p!
                            .NumberPassport
                            .Contains(filterRequest.SearchNumberPassport));
                if (filterRequest.DateBornFrom != null)
                    passports = passports
                        .Where(p => p!
                            .DateBorn >= filterRequest.DateBornFrom);
                if (filterRequest.DateBornTo != null)
                    passports = passports
                        .Where(p => p!
                            .DateBorn <= filterRequest.DateBornTo);
                passportList = passports.ToList();
                var employeeList = employees.ToList();
                if (passportList.Count > 0)
                    employeeList = employeeList.Where(c => passportList.Any(p => c.PassportId == p.Id)).ToList();
                if (filterRequest.CountItem != null && filterRequest.CountItem > 0 && filterRequest.CountItem < employeeList.Count())
                    employeeList = employeeList.Take((int)filterRequest.CountItem).ToList();
                return employeeList.ToList();
            }
            return employees.ToList();
        }
        public Employee GetYoungestEmployee() 
            => _bankContext.Employee
                .First(e => e
                    .Age == _bankContext.Employee
                        .Min(ea => ea.Age));
        public Employee GetOldestEmployee()
            => _bankContext.Employee
                .First(e => e
                    .Age == _bankContext.Employee
                        .Max(ea => ea.Age));
        public int GetAverrageAgeEmployees() 
            => _bankContext.Employee.Any() == false 
            ? _bankContext.Employee.Sum(e => e.Age) / _bankContext.Employee.Count()
            : 0;
        public Employee? GetEmployee(Guid idEmployee)
            => _bankContext.Employee.FirstOrDefault(e => e.Id == idEmployee);
        public string GetSalary(Guid idSalaryCurrency)
        {
            using var db = new BankContext();
            var salary = db.Currency.FirstOrDefault(c => c.Id == idSalaryCurrency);
            return salary is null 
                ? "Информация отсутствует" 
                : $"{salary.Value} {salary.TypeCurrency}";
        }
        public string GetInformation(Employee employee)
        {
            return $"Имя: {employee.Name}\n" +
                $"Возраст: {employee.Age}\n" +
                $"Номер телефона: {employee.NumberPhone}\n" +
                $"Должность: {employee.JobPositionType}\n" +
                $"Заработная плата: {GetSalary(employee.CurrencyIdSalary)}\n" +
                $"Приступил к работе: {employee.StartWorkDate}\n" +
                $"Контракт заканчивается через " +
                $"{Convert.ToDateTime(employee.EndContractDate.ToString()).Subtract(DateTime.Today).TotalDays} дней\n";
        }
        public void AddEmployee(Employee employee)
        {
            employee.Validation();
            using var db = new BankContext();
            if (!db.Passport.Any(p => p.Id == employee.PassportId))
                throw new ArgumentNullException("Такого паспорта не было найденно");
            if (db.Client.Any(p => p.PassportId == employee.PassportId)
                || db.Employee.Any(p => p.PassportId == employee.PassportId))
                throw new ArgumentException("Этот паспорт уже используется");
            db.Employee.Add(employee);
            db.SaveChanges();
        }
        public void ChangeEmployee(Employee employee)
        {
            employee.Validation();
            using var db = new BankContext();
            if (!db.Employee.Any(e => e.Id == employee.Id))
                throw new ArgumentException("Работник не совпадает с изменяемым работником");
            if (!db.Passport.Any(p => p.Id == employee.PassportId))
                throw new ArgumentNullException("Такого паспорта не было найденно");
            db.Employee.Update(employee);
            db.SaveChanges();
        }
        public void DeleteEmployee(Employee employee)
        {
            using var db = new BankContext();
            if (!db.Employee.Any(e => e.Id == employee.Id))
                throw new ArgumentException("Работник не совпадает с удаляемым работником");
            db.Employee.Remove(employee);
            db.SaveChanges();
        }
    }
}
