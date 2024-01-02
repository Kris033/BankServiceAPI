using Models;
using PracticeWithTypes.Extensions;
using Services;

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

            employees.ForEach(e => 
                Console.WriteLine(e.GetEmployeeInformation()));
        }
    }
}