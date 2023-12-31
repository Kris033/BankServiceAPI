using Models;
using System.Diagnostics;

namespace PracticeWithTypes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Employee[] employees = new Employee[20];
            string[] names = new string[] 
            {
                "Алекс", "Олег", "Иван", "Максим", "Алексей", "Михаил", "Виктор", "Николай", "Анастасия", "Александр",
                "Александра", "Татьяна", "Ися", "Григорий", "Дмитрий", "Богдан", "Андрей", "Даниил", "Павел", "Ростислав"
            };
            Random rand = new Random();
            for (int i = 0; i < employees.Length; i++)
            {
                JobPosition job = (JobPosition)
                    rand.Next(0, Enum.GetValues(typeof(JobPosition)).Length);
                Currency salary = new Currency(employees[i], job switch
                {
                    JobPosition.Trainee => 200,
                    JobPosition.Cashier => 1000,
                    JobPosition.Security => 1100,
                    JobPosition.Director => 2000,
                    _ => throw new Exception("Должность не найденна")
                }, CurrencyType.Dollar);
                employees[i] = new Employee(
                    names[i], job, salary,
                    new DateOnly(2021, 7, 21),
                    new DateOnly(2024, 7, 21)
                    );
            }
            foreach (Employee employee in employees )
            {
                employee.UpdateContract(new Contract(
                    employee,
                    "SimpleBank",
                    "c. Cishinau, str. Stefan, h. 2043",
                    "Cishinau",
                    "MD-2000", 
                    "начало работы с 09:00 до 18:00 с понедельника по пятницу",
                    "спустя 11 месяцев отработки сотрудник имеет право взять отпуск сроком до 21 дня"));
            }
        }
    }
}