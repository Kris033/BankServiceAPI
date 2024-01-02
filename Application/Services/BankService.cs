using Models;

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
        public Employee ClientConversionEmployee(Client client) => (Employee)client;
    }
}
