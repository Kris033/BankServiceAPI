using Bogus;
using Models;
using Models.Enums;
using Models.Requests;
using Services.Interfaces;

namespace Services
{
    public class TestDataGenerator
    {
        private Faker _fakerRu = new Faker("ru");
        public async Task<List<Client>> GenerationClients(int count)
        {
            List<Client> clients = new List<Client>();
            for (int i = 0; i < count; i++)
                clients.Add(await GenerationClient());
            return clients;
        }
        public async Task<List<Employee>> GenerationEmployeesAsync(int count)
        {
            List<Employee> employees = new List<Employee>();
            for (int i = 0; i < count; i++)
                employees.Add(await GenerationEmployee());
            return employees;
        }
        public async Task<List<Account>> GenerationAccounts(int count, Client client)
        {
            List<Account> accounts = new List<Account>();
            for (int i = 0; i < count; i++)
                accounts.Add(await GenerationAccount(client));
            return accounts;
        }
        public async Task<Passport> GenerationPassport()
        {
            IPassportService passportService = new PassportService();
            var gender = (GenderType)_fakerRu.Person.Gender;
            var city = _fakerRu.Person.Address.City;
            var passport = new Passport(
                _fakerRu.Name.FirstName((Bogus.DataSets.Name.Gender)gender),
                _fakerRu.Name.LastName((Bogus.DataSets.Name.Gender)gender),
                null,
                gender,
                _fakerRu.Date.BetweenDateOnly(
                    new DateOnly(1980, 6, 20),
                    new DateOnly(2004, 9, 12))
                .ToDateTime(new TimeOnly(0, 0, 0)),
                "город " + city,
                "г. Тирасполь, УВД ПМР, д.24",
                _fakerRu.Date.BetweenDateOnly(
                    new DateOnly(2005, 9, 2),
                    new DateOnly(2024, 1, 13))
                .ToDateTime(new TimeOnly(0, 0, 0)),
                _fakerRu.Random.ReplaceNumbers("1-ПР №0#####"),
                string.Join(", ", "г." + city, _fakerRu.Person.Address.Street, _fakerRu.Person.Address.Suite));
            await passportService.Add(passport);
            return passport;
        }
        public async Task<Employee> GenerationEmployee()
        {
            EmployeeService employeeService = new EmployeeService();
            var passport = await GenerationPassport();
            var salary = await GenerationCurrency();
            Employee employee = new Employee(
                        passport.Id,
                        _fakerRu.Random.ReplaceNumbers("###-####-###"),
                        passport.GetFullName(),
                        passport.GetAge(),
                        (JobPosition)_fakerRu.Random.Number(0, 3),
                        salary.Id,
                        _fakerRu.Date.BetweenDateOnly(
                            new DateOnly(2003, 10, 5),
                            new DateOnly(2024, 1, 3))
                        .ToDateTime(new TimeOnly()),
                        _fakerRu.Date.FutureDateOnly(3)
                        .ToDateTime(new TimeOnly()));
            await employeeService.Add(employee);
            await GenerationContract(employee);
            return employee;
        }
        public async Task<Contract> GenerationContract(Employee employee)
        {
            ContractService contractService = new ContractService();
            var city = _fakerRu.Address.City();
            Contract contract = new Contract(
                    employee.Id,
                    _fakerRu.Company.CompanyName(),
                    string.Join(", ", _fakerRu.Address.CitySuffix() + city, _fakerRu.Address.StreetAddress(), _fakerRu.Address.BuildingNumber()),
                    city,
                    string.Join("-", _fakerRu.Address.CountryCode(), _fakerRu.Address.ZipCode()),
                    "начало работы с 09:00 до 18:00 с понедельника по пятницу",
                    "спустя 11 месяцев отработки сотрудник имеет право взять отпуск сроком до 21 дня");
            await contractService.Add(contract);
            await contractService.SetContract(contract.Id);
            return contract;
        }
        public async Task<Client> GenerationClient()
        {
            IClientService clientService = new ClientService();
            var passport = await GenerationPassport();
            var client = new Client(
                _fakerRu.Random.ReplaceNumbers("###-####-###"),
                passport.Id,
                passport.GetFullName(),
                passport.GetAge());
            await clientService.Add(client);
            return client;
        }
        public async Task<Account> GenerationAccount(Client client)
        {
            IAccountService accountService = new AccountService();
            var currency = await GenerationCurrency();
            var account = new Account(
                client.Id,
                _fakerRu.Random.ReplaceNumbers("#### #### #### ####"),
                currency.Id);
            await accountService.Add(account);
            return account;
        }
        public async Task<Currency> GenerationCurrency()
        {
            ICurrencyService currencyService = new CurrencyService();
            var currency = new Currency(_fakerRu.Random.Number(10, 15000), _fakerRu.PickRandom<CurrencyType>());
            await currencyService.Add(currency);
            return currency;
        }
        public async Task<Dictionary<Client, List<Account>>> GenerationDictionaryAccount(int count)
        {
            var dictionaryAccount = new Dictionary<Client, List<Account>>();
            var clients = await GenerationClients(count);
            foreach (var client in clients)
            {
                var accounts = await GenerationAccounts(_fakerRu.Random.Number(1, 3), client);
                dictionaryAccount.Add(client, accounts);
            }
            return dictionaryAccount;
        }
        public async Task<Dictionary<string, Client>> GetDictionaryPhone(int count)
        {
            ClientService clientService = new ClientService();
            var dictionaryPhoneClients = new Dictionary<string, Client>();
            var clients = await clientService.GetClients(new GetFilterRequest() { CountItem = count });
            clients.ForEach(c => dictionaryPhoneClients.Add(c.NumberPhone, c));
            return dictionaryPhoneClients;
        }
        public async Task<Dictionary<Client, List<Account>>> GetDictionaryAccount(int? count = null)
        {
            ClientService clientService = new ClientService();
            AccountService accountService = new AccountService();
            var dictionaryAccount = new Dictionary<Client, List<Account>>();
            var clients = await clientService.GetClients(new GetFilterRequest() { CountItem = count });
            foreach (var client in clients)
            {
                var accounts = await accountService.GetAccountsClient(client.Id);
                dictionaryAccount.Add(client, accounts);
            }
            return dictionaryAccount;
        }
    }
}