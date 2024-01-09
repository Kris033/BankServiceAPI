using Bogus;
using Models;
using Models.Filters;
using Services.Storage;
using Services.Validations;

namespace Services
{
    public class ClientService
    {
        private readonly Dictionary<Client, List<Account>> _dictionaryAccountsClient;
        private readonly ClientStorage _clients = new ClientStorage();
        public ClientService() 
        {
            _dictionaryAccountsClient = new TestDataGenerator().GenerationDictionaryAccount(50);
        }

        public Client GetFirstClient() => _dictionaryAccountsClient.FirstOrDefault().Key;

        public KeyValuePair<Client, List<Account>> GetFirstClientWithAccounts() 
            => _dictionaryAccountsClient.FirstOrDefault();
        public ClientStorage GetClients(GetFilterRequest? filterRequest = null)
        {
            var clients = _clients.AsQueryable();
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
            return new ClientStorage(clients.ToArray());
        }
        
        public Client GetYoungestClient()
            => _clients
                .First(e => e
                    .Age == _clients
                        .Min(ea => ea.Age));
        public Client GetOldestClient()
            => _clients
                .First(e => e
                    .Age == _clients
                        .Max(ea => ea.Age));
        public int GetAverrageAgeClientss()
            => _clients.Any() == false
            ? _clients.Sum(e => e.Age) / _clients.Count()
            : 0;

        public void AddClient(Client client)
        {
            client.ValidationPerson();
            _clients.Add(client);
            _dictionaryAccountsClient.Add(
                client, 
                new List<Account>()
                {
                    new Account(
                        client,
                        new Faker().Random.ReplaceNumbers("#### #### #### ####"),
                        new Currency(0, CurrencyType.Dollar))
                });
        }
        public void AddAccount(Client client, Account account)
        {
            if (!_dictionaryAccountsClient.ContainsKey(client))
                throw new ArgumentNullException("Такого клиента в реестре банка не существует");
            account.Validation();
            _dictionaryAccountsClient[client].Add(account);
        }
        public void ChangeAccountClient(Client client, Account account)
        {
            if (!_dictionaryAccountsClient.ContainsKey(client))
                throw new ArgumentNullException("Такого клиента в реестре банка не существует");
            if (!_dictionaryAccountsClient[client].Any(a => a.AccountNumber == account.AccountNumber))
                throw new ArgumentNullException("Такого банковского счета у клиента не существует");
            account.Validation();
            _dictionaryAccountsClient[client].ForEach(a => 
            {
                if (a.Equals(account)) a = account;
            });
        }
    }
}
