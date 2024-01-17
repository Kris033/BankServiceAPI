using BankDbConnection;
using Models;
using Models.Enums;

namespace Services
{
    public class BankService
    {
        public Currency CalculationSalaryBetweenDirectors(
            int countDirectors,
            Currency bankProfit,
            Currency costs)
        {

            if(bankProfit.TypeCurrency != CurrencyType.Dollar)
                bankProfit.ExChange(CurrencyType.Dollar);
            if(costs.TypeCurrency != CurrencyType.Dollar)
                costs.ExChange(CurrencyType.Dollar);

            return countDirectors > 0 
                ? new Currency((int)(bankProfit.Value - costs.Value) / countDirectors, CurrencyType.Dollar) 
                : new Currency(0, CurrencyType.Dollar);
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
            var salary = new Currency(0, CurrencyType.Dollar);
            await new CurrencyService().AddCurrency(salary);
            var dateTimeNow = DateTime.UtcNow;
            var dateOnly = new DateOnly(dateTimeNow.Day, dateTimeNow.Month, dateTimeNow.Year);
            var dateOnlyUpp1Year = dateOnly.AddYears(1);
            var employee = new Employee(client.PassportId, client.NumberPhone, client.Name, client.Age, JobPosition.Trainee, salary.Id, dateOnly, dateOnlyUpp1Year);
            await new EmployeeService().AddEmployee(employee);
            return employee;
        }
    }
}
