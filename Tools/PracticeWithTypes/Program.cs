using Models;
using PracticeWithTypes.Extensions;
using Services;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PracticeWithTypes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //List<Employee> employees = new List<Employee>();
            //employees.Capacity = 20;
            //employees.AddRandomEmployees();

            //BankService bankService = new BankService();

            //employees.Add(
            //    bankService
            //    .ClientConversionEmployee(
            //        new Client("Евгений")));
            //employees.UpdateSalaryDirectors(bankService);
            //employees.UpdateContractEmployees();

            ImmitationWorkQueue();


            //CheckTimeBoxingUnBoxing();
        }
        public static void ImmitationWorkQueue()
        {
            Random rand = new Random();
            Queue<Account> queue = new Queue<Account>();
            List<Account> accounts = new List<Account>();
            EventHandler<string> eventHandler = PrintMessage;
            accounts.Capacity = 20;
            List<Client> clients = new List<Client>
            {
                new Client("Алекс"), new Client("Олег"), new Client("Иван"), new Client("Максим"),
                new Client("Алексей"), new Client("Михаил"), new Client("Виктор"), new Client("Николай"), 
                new Client("Анастасия"), new Client("Александр"), new Client("Александра"), new Client("Татьяна"),
                new Client("Изя"), new Client("Григорий"), new Client("Дмитрий"), new Client("Богдан"),
                new Client("Андрей"), new Client("Даниил"), new Client("Павел"), new Client("Ростислав")
            };
            
            for (int i = 0; i < accounts.Capacity; i++)
            {
                var account = new Account(clients[i], new Currency(rand.Next(0, 15000), CurrencyType.LeiMD));
                account.PropertyChanged += PrintMessage;
                account.Notify += PrintMessage;
                clients[i].Account = account;
                accounts.Add(account);
                if (queue.Count > 12)
                    eventHandler?.Invoke(eventHandler, "Превышено мест в очереди, дождитесь пока не появится свободное место");
                else
                    queue.Enqueue(account);
            }
            
            while (queue.Count > 0)
            {
                var account = queue.Peek();
                if(rand.Next(0, 2) == 1)
                    account.Remove(new Currency(rand.Next(10, 300), CurrencyType.Dollar));
                else
                    account.Put(new Currency(rand.Next(10, 500), CurrencyType.Euro));
                queue.Dequeue();
            }
            eventHandler?.Invoke(eventHandler, "Очередь закончена, перерыв.");


        }

        private static void PrintMessage(object? sender, string e) => Console.WriteLine(e);
        public static void PrintMessage(object? sender, PropertyChangedEventArgs e) 
            => Console.WriteLine(e.PropertyName);
        public static void PrintMessage(string message) => Console.WriteLine(message);
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