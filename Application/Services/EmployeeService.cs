using Models;
using Services.Storage;
using Models.Validations;
using BankDbConnection;
using Models.Requests;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services
{
    public class EmployeeService : IEmployeeService
    {
        public EmployeeService() { }
        public async Task<Employee?> GetFirstEmployee() 
        {
            using var db = new BankContext();
            return await db.Employee.FirstOrDefaultAsync();
        }
        public async Task<List<Employee>> GetEmployees(GetFilterRequest? filterRequest = null)
        {
            using var db = new BankContext();
            var employees = db.Employee.AsQueryable();
            if (filterRequest != null)
            {
                var passportService = new PassportService();
                List<Passport> passportList = new List<Passport>();
                await employees.ForEachAsync(async c =>
                {
                    var passport = await passportService.Get(c.PassportId);
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
                var employeeList = employees.ToList();
                employeeList = employeeList.Where(c => passports.Any(p => c.PassportId == p.Id)).ToList();
                if (filterRequest.CountItem != null 
                    && filterRequest.CountItem > 0 
                    && filterRequest.CountItem < employeeList.Count())
                    employeeList = employeeList.Take((int)filterRequest.CountItem).ToList();
                return employeeList;
            }
            return employees.ToList();
        }
        public async Task<Employee> GetYoungestEmployee()
        {
            using var db = new BankContext();
            int minAgeEmployee = await db.Employee.MinAsync(e => e.Age);
            return await db.Employee.FirstAsync(e => e.Age == minAgeEmployee);
        }
        public async Task<Employee> GetOldestEmployee()
        {
            using var db = new BankContext();
            int maxAgeEmployee = await db.Employee.MaxAsync(e => e.Age);
            return await db.Employee.FirstAsync(e => e.Age == maxAgeEmployee);
        }
        public async Task<int> GetAverrageAgeEmployees()
        {
            using var db = new BankContext();
            return await db.Employee.AnyAsync() == false
            ? await db.Employee.SumAsync(e => e.Age)
            / await db.Employee.CountAsync()
            : 0;
        }
        public async Task<Employee?> Get(Guid idEmployee)
        {
            using var db = new BankContext();
            return await db.Employee.FirstOrDefaultAsync(e => e.Id == idEmployee);
        }
           
        public async Task<string> GetStringSalary(Guid idSalaryCurrency)
        {
            using var db = new BankContext();
            var salary = await db.Currency.FirstOrDefaultAsync(c => c.Id == idSalaryCurrency);
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
                $"Заработная плата: {GetStringSalary(employee.CurrencyIdSalary)}\n" +
                $"Приступил к работе: {employee.StartWorkDate}\n" +
                $"Контракт заканчивается через " +
                $"{Convert.ToDateTime(employee.EndContractDate.ToString()).Subtract(DateTime.Today).TotalDays} дней\n";
        }
        public async Task Add(Employee employee)
        {
            employee.Validation();
            using var db = new BankContext();
            if (!await db.Passport.AnyAsync(p => p.Id == employee.PassportId))
                throw new ArgumentNullException("Такого паспорта не было найденно");
            if (await db.Client.AnyAsync(p => p.PassportId == employee.PassportId)
                || await db.Employee.AnyAsync(p => p.PassportId == employee.PassportId))
                throw new ArgumentException("Этот паспорт уже используется");
            db.Employee.Add(employee);
            await db.SaveChangesAsync();
        }
        public async Task Update(Employee employee)
        {
            employee.Validation();
            using var db = new BankContext();
            if (!await db.Employee.AnyAsync(e => e.Id == employee.Id))
                throw new ArgumentException("Работник не совпадает с изменяемым работником");
            if (!await db.Passport.AnyAsync(p => p.Id == employee.PassportId))
                throw new ArgumentNullException("Такого паспорта не было найденно");
            db.Employee.Update(employee);
            await db.SaveChangesAsync();
        }
        public async Task Delete(Guid idEmployee)
        {
            using var db = new BankContext();
            var employee = await db.Employee.FirstOrDefaultAsync(e => e.Id == idEmployee);
            if (employee == null)
                throw new ArgumentNullException("Работник не совпадает с удаляемым работником");
            db.Employee.Remove(employee);
            await db.SaveChangesAsync();
        }
    }
}
