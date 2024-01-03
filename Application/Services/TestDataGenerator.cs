using Bogus;
using Models;

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
        public List<Employee> GenerationEmployee(int count)
            => Enumerable.Range(1, count).Select(_ => 
                new Employee(
                    _fakerRu.Name.FullName(),
                    (JobPosition)_fakerRu.Random.Number(0, 3),
                    new Currency(_fakerRu.Random.Number(120, 2500), CurrencyType.Dollar),
                    _fakerRu.Date.BetweenDateOnly(new DateOnly(2003, 10, 5), new DateOnly(2024, 1, 3)),
                    _fakerRu.Date.FutureDateOnly(3)))
                    .ToList();
        public Dictionary<string, Client> GenerationDictionaryPhone(int count)
        {
            var dictionaryPhoneClients = new Dictionary<string, Client>();
            var clients = GenerationClients(count);
            clients.ForEach(c => dictionaryPhoneClients.Add(c.NumberPhone, c));
            return dictionaryPhoneClients;
        }
    }
}
