using Models;
using PracticeWithTypes.Extensions;
using Services;
using System.Diagnostics;

namespace PracticeWithTypes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Employee> employees = new List<Employee>();
            employees.Capacity = 20;
            employees.AddRandomEmployees();

            BankService bankService = new BankService();

            employees.Add(
                bankService
                .ClientConversionEmployee(
                    new Client("Евгений")));
            employees.UpdateSalaryDirectors(bankService);
            employees.UpdateContractEmployees();

            CheckTimeBoxingUnBoxing();
        }
        public static void CheckTimeBoxingUnBoxing()
        {
            Stopwatch stopwatch = new Stopwatch();
            Client client = new Client("Ксения");
            stopwatch.Start();
            object oClient = client;
            stopwatch.Stop();
            Console.WriteLine("Упаковка: " + stopwatch.Elapsed.Ticks); // 25 - ~36
            stopwatch.Restart();
            client = (Client)oClient;
            stopwatch.Stop();
            Console.WriteLine("Распаковка: " + stopwatch.Elapsed.Ticks); // 7 - ~12
        }
    }
}