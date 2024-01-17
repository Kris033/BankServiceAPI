using Bogus;
using Models;
using Models.Enums;

namespace Services
{
    public class TestDataGenerator
    {
        private Faker _fakerRu = new Faker("ru");
        public async Task<List<Client>> GenerationClients(int count)
        {
            ClientService clientService = new ClientService();
            CurrencyService currencyService = new CurrencyService();
            PassportService passportService = new PassportService();
            List<Client> clients = new List<Client>();
            for (int i = 0; i < count; i++)
            {
                var passport = GenerationPassport();
                await passportService.AddPassport(passport);
                var person = new Models.Person(passport.Id, _fakerRu.Random.ReplaceNumbers("###-####-###"), passport.GetFullName(), passport.GetAge());
                var currency = new Currency(_fakerRu.Random.Number(10, 5000), _fakerRu.PickRandom<CurrencyType>());
                Client client = new Client(person.NumberPhone, person.PassportId, person.Name, person.Age);
                await clientService.AddClient(client);
                clients.Add(client);
                await currencyService.AddCurrency(currency);
                await clientService.AddAccount(new Account(client.Id, _fakerRu.Random.ReplaceNumbers("#### #### #### ####"), currency.Id));
            }
            return clients;
        }
        public async Task<List<Employee>> GenerationEmployees(int count)
        {
            EmployeeService clientService = new EmployeeService();
            CurrencyService currencyService = new CurrencyService();
            PassportService passportService = new PassportService();
            List<Employee> employees = new List<Employee>();
            employees.Capacity = count;
            for (int i = 0; i < count; i++)
            {
                var passport = GenerationPassport();
                await passportService.AddPassport(passport);
                var person = new Models.Person(passport.Id, _fakerRu.Random.ReplaceNumbers("###-####-###"), passport.GetFullName(), passport.GetAge());
                var currency = new Currency(_fakerRu.Random.Number(10, 5000), _fakerRu.PickRandom<CurrencyType>());
                await currencyService.AddCurrency(currency);
                Employee employee = new Employee(
                        person.PassportId,
                        person.NumberPhone,
                        person.Name,
                        person.Age,
                        (JobPosition)_fakerRu.Random.Number(0, 3),
                        currency.Id,
                        _fakerRu.Date.BetweenDateOnly(new DateOnly(2003, 10, 5), new DateOnly(2024, 1, 3)),
                        _fakerRu.Date.FutureDateOnly(3));
                await clientService.AddEmployee(employee);
                employees.Add(employee);
            }
            return employees;
        }
        public async Task<List<Account>> GenerationAccounts(int count, Client? client = null)
        {
            ClientService clientService = new ClientService();
            CurrencyService currencyService = new CurrencyService();
            PassportService passportService = new PassportService();
            List<Account> accounts = new List<Account>();
            accounts.Capacity = count;
            if (client == null)
            {
                var passport = GenerationPassport();
                await passportService.AddPassport(passport);
                client = new Client(_fakerRu.Random.ReplaceNumbers("###-####-###"), passport.Id, passport.GetFullName(), passport.GetAge());
                await clientService.AddClient(client);
            }
            for (int i = 0; i < count; i++)
            {
                var currency = new Currency(_fakerRu.Random.Number(10, 5000), _fakerRu.PickRandom<CurrencyType>());
                await currencyService.AddCurrency(currency);
                var account = new Account(client.Id, _fakerRu.Random.ReplaceNumbers("#### #### #### ####"), currency.Id);
                await clientService.AddAccount(account);
                accounts.Add(account);
            }
            return accounts;
        }
        public Passport GenerationPassport()
        {
            var gender = (GenderType)_fakerRu.Person.Gender;
            var city = _fakerRu.Person.Address.City;
            return new Passport(
                _fakerRu.Name.FirstName((Bogus.DataSets.Name.Gender?)gender),
                _fakerRu.Name.LastName((Bogus.DataSets.Name.Gender?)gender),
                null,
                gender,
                _fakerRu.Date.BetweenDateOnly(
                    new DateOnly(1980, 6, 20),
                    new DateOnly(2004, 9, 12)),
                "город " + city, "г. Тирасполь, УВД ПМР, д.24",
                _fakerRu.Date.BetweenDateOnly(
                    new DateOnly(2005, 9, 2),
                    new DateOnly(2024, 1, 13)),
                _fakerRu.Random.ReplaceNumbers("1-ПР №0#####"),
                string.Join(", ", "г." + city, _fakerRu.Person.Address.Street, _fakerRu.Person.Address.Suite));
        }
        public async Task<Dictionary<string, Client>> GenerationDictionaryPhone(int count)
        {
            var dictionaryPhoneClients = new Dictionary<string, Client>();
            var clients = await GenerationClients(count);
            clients.ForEach(c => dictionaryPhoneClients.Add(c.NumberPhone, c));
            return dictionaryPhoneClients;
        }
        public async Task<Dictionary<Client, List<Account>>> GenerationDictionaryAccount(int count)
        {
            var dictionaryAccount = new Dictionary<Client, List<Account>>();
            var accounts = await GenerationAccounts(count);
            
            //accounts.ForEach(a => 
            //    dictionaryAccount
            //        .Add(a.Client, 
            //            GenerationAccounts(_fakerRu.Random.Number(1, 4), a.Client)));

            return dictionaryAccount;
        }
    }
}
