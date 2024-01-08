using Bogus;
using Models;
using Services.Exceptions;
using Services.Validations;

namespace Services
{
    public class ClientService
    {
        private readonly Dictionary<Client, List<Account>> _dictionaryAccountsClient;
        public ClientService() 
        {
            _dictionaryAccountsClient = new TestDataGenerator().GenerationDictionaryAccount(50);
        }

        public Client GetFirstClient() => _dictionaryAccountsClient.FirstOrDefault().Key;

        public KeyValuePair<Client, List<Account>> GetFirstClientWithAccounts() 
            => _dictionaryAccountsClient.FirstOrDefault();

        public void AddClient(Client client)
        {
            client.ValidationPerson();
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
