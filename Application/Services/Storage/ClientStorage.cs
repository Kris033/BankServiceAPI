using BankDbConnection;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections;

namespace Services.Storage
{
    public class ClientStorage : IEnumerable<Client>, IClientStorage
    {
        private async Task<Client?> GetClientById(Guid idClient) 
        {
            using var db = new BankContext();
            return await db.Client.FirstOrDefaultAsync(c => c.Id == idClient);
        }
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

        public async void Update(Client client)
        {
            var foundClient = await GetClientById(client.Id);
            if(foundClient != null)
            {
                List<Account> listAccounts = Data[foundClient];
                listAccounts.ForEach(account =>
                {
                    account.ClientId = client.Id;
                });
                if (Data.Remove(foundClient))
                    Data.Add(client, listAccounts);
            }
        }
        public void Delete(Client client)
        {
            Data.Remove(client);
        }

        public async Task AddAccount(Account account)
        {
            var client = await GetClientById(account.ClientId);
            if(client != null) Data[client].Add(account);
        }

        public async Task UpdateAccount(Account account)
        {
            var client = await GetClientById(account.ClientId);
            if (client != null) 
                Data[client].ForEach(a =>
                {
                    if (a.AccountNumber == account.AccountNumber) a = account;
                });
        }

        public async Task DeleteAccount(Account account)
        {
            var client = await GetClientById(account.ClientId);
            if (client != null) Data[client].Remove(account);
        }

    }
}