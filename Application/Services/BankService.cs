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
        public void AddToBlackList<T>(T person) where T : Person
        {
            if (IsPersonInBlackList(person)) return;
            person.InBlackList = true;
        }
        public bool IsPersonInBlackList<T>(T person) where T : Person
            => person.InBlackList;
        public Employee ClientConversionEmployee(Client client)
        {
            var idSalary = new CurrencyService().AddCurrency(new Currency(0, CurrencyType.Dollar)).Id;
            var dateTimeNow = DateTime.UtcNow;
            var dateOnly = new DateOnly(dateTimeNow.Day, dateTimeNow.Month, dateTimeNow.Year);
            var dateOnlyUpp1Year = dateOnly.AddYears(1);
            return new Employee(client.PassportId, client.NumberPhone, client.Name, client.Age, JobPosition.Trainee, idSalary, dateOnly, dateOnlyUpp1Year);
        }
    }
}
