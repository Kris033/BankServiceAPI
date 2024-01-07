using Bogus;
using Models;
using System.Security.Cryptography;
using System.Text;

namespace Services
{
    public class TestDataGenerator
    {
        private Faker _fakerRu = new Faker("ru");
        public List<Client> GenerationClients(int count) 
            => Enumerable
                .Range(1, count)
                .Select(_ => new Client(
                    _fakerRu.Name.FullName(),
                    _fakerRu.Random.Number(15, 90),
                    _fakerRu.Random.ReplaceNumbers("###-####-###")))
                .ToList();
        public List<Employee> GenerationEmployees(int count)
            => Enumerable.Range(1, count).Select(_ => 
                new Employee(
                    _fakerRu.Name.FullName(),
                    _fakerRu.Random.Number(18, 60),
                    _fakerRu.Random.ReplaceNumbers("###-####-###"),
                    (JobPosition)_fakerRu.Random.Number(0, 3),
                    new Currency(_fakerRu.Random.Number(120, 2500), CurrencyType.Dollar),
                    _fakerRu.Date.BetweenDateOnly(new DateOnly(2003, 10, 5), new DateOnly(2024, 1, 3)),
                    _fakerRu.Date.FutureDateOnly(3)))
                    .ToList();
        public List<Account> GenerationAccounts(int count, Client? client = null)
            => client is null 
            ? Enumerable.Range(1, count).Select(_ =>
                new Account(
                    new Client(
                        _fakerRu.Name.FullName(),
                        _fakerRu.Random.Number(15, 90),
                        _fakerRu.Random.ReplaceNumbers("###-####-###")),
                    _fakerRu.Random.ReplaceNumbers("#### #### #### ####"),
                    new Currency(
                        _fakerRu.Random.Number(10, 5000),
                        _fakerRu.PickRandom<CurrencyType>())))
                    .ToList()
            : Enumerable.Range(1, count).Select(_ =>
                new Account(
                    client,
                    _fakerRu.Random.ReplaceNumbers("#### #### #### ####"),
                    new Currency(
                        _fakerRu.Random.Number(10, 5000),
                        _fakerRu.PickRandom<CurrencyType>())))
                    .ToList();
        public Dictionary<string, Client> GenerationDictionaryPhone(int count)
        {
            var dictionaryPhoneClients = new Dictionary<string, Client>();
            var clients = GenerationClients(count);
            clients.ForEach(c => dictionaryPhoneClients.Add(c.NumberPhone, c));
            return dictionaryPhoneClients;
        }
        public Dictionary<Client, List<Account>> GenerationDictionaryAccount(int count)
        {
            var dictionaryAccount = new Dictionary<Client, List<Account>>();
            var accounts = GenerationAccounts(count);
            
            accounts.ForEach(a => 
                dictionaryAccount
                    .Add(a.Client, 
                        GenerationAccounts(_fakerRu.Random.Number(1, 4), a.Client)));

            return dictionaryAccount;
        }
    }
}
