using Models;

namespace Services
{
    public class BankService
    {
        private List<Person> BlackList = new List<Person>();
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
            BlackList.Add(person);
        }
        public bool IsPersonInBlackList<T>(T person) where T : Person 
            => BlackList.Contains(person);
        public Employee ClientConversionEmployee(Client client) => (Employee)client;
    }
}
