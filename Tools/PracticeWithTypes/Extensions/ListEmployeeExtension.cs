using Models;
using Services;

namespace PracticeWithTypes.Extensions
{
    public static class ListEmployeeExtension
    {
        public static void UpdateContractEmployees(this List<Employee> listEmployee)
        {
            listEmployee.ForEach(e => e.UpdateContract(new Contract(
                    e,
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

            listEmployee.ForEach(e =>
            {
                if (e.JobPositionType == JobPosition.Director)
                    e.UpdateSalary(salaryDirectors);
            });
        }
        //public static void AddRandomEmployees(this List<Employee> listEmployee)
        //{
        //    string[] names = new string[]
        //    {
        //        "Алекс", "Олег", "Иван", "Максим", "Алексей", "Михаил", "Виктор", "Николай", "Анастасия", "Александр",
        //        "Александра", "Татьяна", "Ися", "Григорий", "Дмитрий", "Богдан", "Андрей", "Даниил", "Павел", "Ростислав"
        //    };
        //    Random rand = new Random();
        //    var countJobPositions = Enum.GetValues(typeof(JobPosition)).Length;
        //    for (int i = 0; i < listEmployee.Capacity; i++)
        //    {
        //        JobPosition job = (JobPosition)
        //            rand.Next(0, countJobPositions);
        //        listEmployee.Add(new Employee(
        //            names[i], job,
        //            new DateOnly(2021, 7, 21),
        //            new DateOnly(2024, 7, 21)
        //            ));
        //    }
        //}
    }
}
