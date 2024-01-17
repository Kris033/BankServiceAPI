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
            listEmployee.ForEach(async e => await contractService.UpdateContract(new Contract(
                    e.Id,
                    "SimpleBank",
                    "c. Cishinau, str. Stefan, h. 2043",
                    "Cishinau",
                    "MD-2000",
                    "начало работы с 09:00 до 18:00 с понедельника по пятницу",
                    "спустя 11 месяцев отработки сотрудник имеет право взять отпуск сроком до 21 дня")));
        }

        public static void UpdateSalaryDirectors(this List<Employee> listEmployee, BankService bankService)
        {
            Currency salaryDirectors = bankService.CalculationSalaryBetweenDirectors(
                listEmployee.Count(
                    e => e.JobPositionType == JobPosition.Director),
                new Currency(800000, CurrencyType.LeiMD),
                new Currency(150000, CurrencyType.LeiMD));
            var currencyService = new CurrencyService();
            listEmployee.ForEach(async e =>
            {
                if (e.JobPositionType == JobPosition.Director)
                {
                    var currency = await currencyService.GetCurrency(e.CurrencyIdSalary);
                    if (salaryDirectors.TypeCurrency != currency!.TypeCurrency)
                        currency.ExChange(salaryDirectors.TypeCurrency);
                    currency.ChangeValue(salaryDirectors.Value);
                    await currencyService.UpdateCurrency(salaryDirectors);
                }
            });
        }
    }
}
