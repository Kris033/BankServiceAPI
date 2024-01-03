using Models;
using PracticeWithTypes.Extensions;
using Services;
using System.ComponentModel;
using System.Diagnostics;

namespace PracticeWithTypes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dataGenerator = new TestDataGenerator();
            var employees = dataGenerator.GenerationEmployee(1000);
            var clients = dataGenerator.GenerationClients(1000);
            var dictionaryPhoneClients = dataGenerator.GenerationDictionaryPhone(1000);

            BankService bankService = new BankService();
            employees.UpdateSalaryDirectors(bankService);
            employees.UpdateContractEmployees();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            clients.Any(c => c.NumberPhone == "123-4567-890");
            stopwatch.Stop();
            Console.WriteLine(GetResultTimeSearch(stopwatch));
            stopwatch.Restart();
            dictionaryPhoneClients.Any(d => d.Key == "430-2113-234");
            stopwatch.Stop();
            Console.WriteLine(GetResultTimeSearch(stopwatch)); // ~5мс - ~100мс

            Console.WriteLine("--Клиенты младше 18 лет--");
            clients
                .Where(c => c.Age < 18)
                .ToList()
                .ForEach(c => Console.WriteLine(c.GetClient()));
            Console.WriteLine($"Самая минимальная заработная плата: {employees.Min(e => e.Salary.Value)} {CurrencyType.Dollar}");

            var lastElementDictionary = dictionaryPhoneClients.Last();
            stopwatch.Restart();
            dictionaryPhoneClients.FirstOrDefault(d => d.Key == lastElementDictionary.Key);
            stopwatch.Stop();
            Console.WriteLine(GetResultTimeSearch(stopwatch));
            stopwatch.Restart();
            dictionaryPhoneClients.Where(d => d.Key == lastElementDictionary.Key);
            stopwatch.Stop();
            Console.WriteLine(GetResultTimeSearch(stopwatch));
        }
        public static string GetResultTimeSearch(Stopwatch stopwatch) => "Поиск занял: " + stopwatch.Elapsed.TotalMilliseconds + " мс";
        public static void ImmitationWorkQueue()
        {
            Random rand = new Random();
            Queue<Account> queue = new Queue<Account>();
            List<Account> accounts = new List<Account>();
            EventHandler<string> eventHandler = PrintMessage;
            accounts.Capacity = 20;
            List<Client> clients = new TestDataGenerator().GenerationClients(20);
            
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

    }
}