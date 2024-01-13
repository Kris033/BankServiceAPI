using BankDbConnection;
using Models;
using System.Collections;

namespace Services.Storage
{
    public class ClientStorage : IEnumerable<Client>, IClientStorage
    {
        private Client? GetClientById(Guid idClient)
            => new BankContext().Client.FirstOrDefault(c => c.Id == idClient);
        public Dictionary<Client, List<Account>> Data { get; }
        public ClientStorage(Dictionary<Client, List<Account>> clientsAccount)
        {
            Data = clientsAccount;
        }
        public ClientStorage()
        {
            Data = new Dictionary<Client, List<Account>>();
        }
        public List<Account> this[Client client] => Data[client];
        public IEnumerator<Client> GetEnumerator()
            => ((IEnumerable<Client>)Data).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(Client client)
        {
            Data.Add(client, new List<Account>());
        }

        public void Update(Client client)
        {
            var foundClient = Data.First(ca => ca.Key.NumberPhone == client.NumberPhone);
            foundClient.Value.ForEach(a => 
            {
                a.ClientId = client.Id;
            });
            if (Data.Remove(foundClient.Key))
                Data.Add(client, foundClient.Value);
        }
        public void Delete(Client client)
        {
            Data.Remove(client);
        }

        public void AddAccount(Account account)
        {
            var client = GetClientById(account.ClientId);
            if(client != null) Data[client].Add(account);
        }

        public void UpdateAccount(Account account)
        {
            var client = GetClientById(account.ClientId);
            if (client != null) 
                Data[client].ForEach(a =>
                {
                    if (a.AccountNumber == account.AccountNumber) a = account;
                });
        }

        public void DeleteAccount(Account account)
        {
            var client = GetClientById(account.ClientId);
            if (client != null) Data[client].Remove(account);
        }

    }
}
