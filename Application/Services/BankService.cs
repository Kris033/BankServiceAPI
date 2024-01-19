using BankDbConnection;
using Models;
using Models.Enums;

namespace Services
{
    public class BankService
    {
        public async Task<Currency> CalculationSalaryBetweenDirectors(
            int countDirectors,
            Currency bankProfit,
            Currency costs)
        {
            CurrencyService currencyService = new CurrencyService();
            if (bankProfit.TypeCurrency != CurrencyType.USD)
                await currencyService.ExChange(bankProfit, CurrencyType.USD);
            if (costs.TypeCurrency != CurrencyType.USD)
                await currencyService.ExChange(costs, CurrencyType.USD);

            return countDirectors > 0 
                ? new Currency((int)(bankProfit.Value - costs.Value) / countDirectors, CurrencyType.USD) 
                : new Currency(0, CurrencyType.USD);
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
            await new CurrencyService().AddCurrency(salary);
            var dateTimeNow = DateTime.UtcNow;
            var dateTimeUpp1Year = dateTimeNow.AddYears(1);
            var employee = new Employee(client.PassportId, client.NumberPhone, client.Name, client.Age, JobPosition.Trainee, salary.Id, dateTimeNow, dateTimeUpp1Year);
            await new EmployeeService().AddEmployee(employee);
            return employee;
        }
    }
}
