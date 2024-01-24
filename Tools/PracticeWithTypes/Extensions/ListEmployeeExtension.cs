using Models;
using Models.Enums;
using Services;

namespace PracticeWithTypes.Extensions
{
    public static class ListEmployeeExtension
    {

        public static void UpdateContractEmployees(this List<Employee> listEmployee)
        {
            var contractService = new ContractService();
            listEmployee.ForEach(async e => await contractService.Update(new Contract(
                    e.Id,
                    "SimpleBank",
                    "c. Cishinau, str. Stefan, h. 2043",
                    "Cishinau",
                    "MD-2000",
                    "начало работы с 09:00 до 18:00 с понедельника по пятницу",
                    "спустя 11 месяцев отработки сотрудник имеет право взять отпуск сроком до 21 дня")));
        }

        public static async Task UpdateSalaryDirectors(this List<Employee> listEmployee, BankService bankService)
        {
            Currency salaryDirectors = await bankService.CalculationSalaryBetweenDirectors(
                new Currency(800000, CurrencyType.MDL),
                new Currency(150000, CurrencyType.MDL));
            var currencyService = new CurrencyService();
            listEmployee.ForEach(async e =>
            {
                if (e.JobPositionType == JobPosition.Director)
                {
                    var currency = await currencyService.Get(e.CurrencyIdSalary);
                    if (salaryDirectors.TypeCurrency != currency!.TypeCurrency)
                        await currencyService.ExChange(currency, salaryDirectors.TypeCurrency);
                    await currencyService.Update(salaryDirectors);
                }
            });
        }
    }
}
