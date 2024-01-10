using Bogus;
using Models;
using Models.Filters;
using Services.Storage;
using Models.Validations;

namespace Services
{
    public class ClientService
    {
        private readonly IClientStorage _accountsClient;
        public ClientService() 
        {
            _accountsClient = new ClientStorage(
                new TestDataGenerator()
                    .GenerationDictionaryAccount(50));
        }

        public Client GetFirstClient() => _accountsClient.Data.FirstOrDefault().Key;

        public KeyValuePair<Client, List<Account>> GetFirstClientWithAccounts() 
            => _accountsClient.Data.FirstOrDefault();
        public List<Client> GetClients(GetFilterRequest? filterRequest = null)
        {
            var clients = _accountsClient
                .Data
                .Keys
                .AsQueryable();
            if (filterRequest != null)
            {
                if (!string.IsNullOrWhiteSpace(filterRequest.SearchFullName))
                    clients = clients
                        .Where(e => e
                            .Name
                            .Contains(filterRequest.SearchFullName));
                if (!string.IsNullOrWhiteSpace(filterRequest.SearchNumberPhone))
                    clients = clients
                        .Where(e => e
                            .NumberPhone
                            .Contains(filterRequest.SearchNumberPhone));
                if (!string.IsNullOrWhiteSpace(filterRequest.SearchNumberPassport))
                    clients = clients
                        .Where(e => e
                            .Passport!
                            .NumberPassport
                            .Contains(filterRequest.SearchNumberPassport));
                if (filterRequest.DateBornFrom != null)
                    clients = clients
                        .Where(e => e
                            .Passport!
                            .DateBorn >= filterRequest.DateBornFrom);
                if (filterRequest.DateBornTo != null)
                    clients = clients
                        .Where(e => e
                            .Passport!
                            .DateBorn <= filterRequest.DateBornTo);
            }
            return clients.ToList();
        }
        
        public Client GetYoungestClient()
            => _accountsClient.Data.Keys
                .First(e => e
                    .Age == _accountsClient.Data.Keys
                        .Min(ea => ea.Age));
        public Client GetOldestClient()
            => _accountsClient.Data.Keys
                .First(e => e
                    .Age == _accountsClient.Data.Keys
                        .Max(ea => ea.Age));
        public int GetAverrageAgeClientss()
            => _accountsClient.Data.Keys.Any()
            ? _accountsClient.Data.Keys.Sum(e => e.Age) / _accountsClient.Data.Keys.Count
            : 0;

        public void AddClient(Client client)
        {
            client.Validation();
            _accountsClient.Add(client);
            _accountsClient
                .AddAccount(
                    new Account(
                        client,
                        new Faker().Random.ReplaceNumbers("#### #### #### ####"),
                        new Currency(0, CurrencyType.Dollar)));
        }
        public void AddAccount(Client client, Account account)
        {
            if (!_accountsClient.Data.ContainsKey(client))
                throw new ArgumentNullException("Такого клиента в реестре банка не существует");
            account.Validation();
            _accountsClient.AddAccount(account);
        }
        public void ChangeAccountClient(Client client, Account account)
        {
            if (!_accountsClient.Data.ContainsKey(client))
                throw new ArgumentNullException("Такого клиента в реестре банка не существует");
            if (!_accountsClient.Data[client].Any(a => a.AccountNumber == account.AccountNumber))
                throw new ArgumentNullException("Такого банковского счета у клиента не существует");
            account.Validation();
            _accountsClient.UpdateAccount(account);
        }
    }
}
