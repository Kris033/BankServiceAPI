using BankDbConnection;
using Models;
using Models.Enums;

namespace Services
{
    public class BankService
    {
        public async Task<Currency> CalculationSalaryBetweenDirectors(
            Currency bankProfit,
            Currency costs)
        {
            CurrencyService currencyService = new CurrencyService();
            EmployeeService employeeService = new EmployeeService();

            var directors = await employeeService.GetEmployees();
            directors = directors
                .Where(e => e.JobPositionType == JobPosition.Director)
                .ToList();

            if (bankProfit.TypeCurrency != CurrencyType.USD)
                await currencyService.ExChange(bankProfit, CurrencyType.USD);
            if (costs.TypeCurrency != CurrencyType.USD)
                await currencyService.ExChange(costs, CurrencyType.USD);

            Currency newSalary = new(
                directors.Count != 0 
                ? (bankProfit.Value - costs.Value) / directors.Count
                : 0,
                CurrencyType.USD);
            newSalary.ChangeValue(newSalary.Value);

            foreach (var director in directors)
            {
                var salary = await currencyService.Get(director.CurrencyIdSalary);
                if (salary != null)
                {
                    if (salary.TypeCurrency != CurrencyType.USD)
                        await currencyService.ExChange(salary, CurrencyType.USD);
                    salary.ChangeValue(newSalary.Value);
                    await currencyService.Update(salary);
                }
                else
                {
                    salary = new Currency(newSalary.Value, newSalary.TypeCurrency);
                    await currencyService.Add(newSalary);
                    director.CurrencyIdSalary = salary.Id;
                    await employeeService.Update(director);
                }
            }
            return newSalary;
        }
        public async Task AddToBlackList<T>(T person) where T : Person
        {
            using var db = new BankContext();
            if (IsPersonInBlackList(person)) return;
            person.InBlackList = true;
            await db.SaveChangesAsync();
        }
        public bool IsPersonInBlackList<T>(T person) where T : Person
            => person.InBlackList;
        public async Task<Employee> ClientConversionEmployee(Client client)
        {
            var salary = new Currency(0, CurrencyType.USD);
            await new CurrencyService().Add(salary);
            var dateTimeNow = DateTime.UtcNow;
            var dateTimeUpp1Year = dateTimeNow.AddYears(1);
            var employee = new Employee(client.PassportId, client.NumberPhone, client.Name, client.Age, JobPosition.Trainee, salary.Id, dateTimeNow, dateTimeUpp1Year);
            await new EmployeeService().Add(employee);
            return employee;
        }
    }
}
