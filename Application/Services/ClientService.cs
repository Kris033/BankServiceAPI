using Models;
using Services.Storage;
using Models.Validations;
using BankDbConnection;
using Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class ClientService
    {
        private readonly IClientStorage _accountsClient;
        public ClientService() 
        {
            _accountsClient = new ClientStorage();
        }
        public Client GetFirstClient() => _accountsClient.Data.FirstOrDefault().Key;

        public KeyValuePair<Client, List<Account>> GetFirstClientWithAccounts() 
            => _accountsClient.Data.FirstOrDefault();
        public async Task<List<Client>> GetClients(GetFilterRequest? filterRequest = null)
        {
            using var db = new BankContext();
            var clients = db.Client.AsQueryable();
            if (filterRequest != null)
            {
                var passportService = new PassportService();
                List<Passport> passportList = new List<Passport>();
                await clients.ForEachAsync(async c =>
                {
                    var passport = await passportService.GetPassport(c.PassportId);
                    if (passport != null) passportList.Add(passport);
                });
                var passports = passportList.AsQueryable();
                if (!string.IsNullOrWhiteSpace(filterRequest.SearchFullName))
                    clients = clients
                        .Where(c => c
                            .Name
                            .Contains(filterRequest.SearchFullName));
                if (!string.IsNullOrWhiteSpace(filterRequest.SearchNumberPhone))
                    clients = clients
                        .Where(c => c
                            .NumberPhone
                            .Contains(filterRequest.SearchNumberPhone));
                if (!string.IsNullOrWhiteSpace(filterRequest.SearchNumberPassport))
                    passports = passports
                        .Where(p => p!
                            .NumberPassport
                            .Contains(filterRequest.SearchNumberPassport));
                if (filterRequest.DateBornFrom != null)
                    passports = passports
                        .Where(p => p!
                            .DateBorn >= filterRequest.DateBornFrom);
                if (filterRequest.DateBornTo != null)
                    passports = passports
                        .Where(p => p!
                            .DateBorn <= filterRequest.DateBornTo);
                passportList = passports.ToList();
                var clientList = clients.ToList();
                if (passportList.Count > 0)
                    clientList = clientList.Where(c => passportList.Any(p => c.PassportId == p.Id)).ToList();
                if (filterRequest.CountItem != null && filterRequest.CountItem > 0 && filterRequest.CountItem < clientList.Count())
                    clientList = clientList.Take((int)filterRequest.CountItem).ToList();
                return clientList.ToList();
            }
            return clients.ToList();
        }

        public async Task<Client> GetYoungestClient()
        {
            using var db = new BankContext();
            int maxAgeFromDb = await db.Client.MaxAsync(ea => ea.Age);
            return await db.Client.FirstAsync(e => e.Age == maxAgeFromDb);
        }
        public async Task<Client> GetOldestClient()
        {
            using var db = new BankContext();
            int maxAgeFromDb = await db.Client.MaxAsync(ea => ea.Age);
            return await db.Client.FirstAsync(e => e.Age == maxAgeFromDb);
        }
        public async Task<int> GetAverrageAgeClients()
        {
            using var db = new BankContext();
            return await db.Client.AnyAsync()
            ? await db.Client.SumAsync(e => e.Age)
            / await db.Client.CountAsync()
            : 0;
        }
        public string GetInformation(Client client)
        {
            return $"Имя: {client.Name}\n" +
                $"Возраст: {client.Age}\n" +
                $"Номер телефона: {client.NumberPhone}\n";
        }
        public async Task<Client?> GetClient(Guid idClient)
        {
            using var db = new BankContext();
            return await db.Client.FirstOrDefaultAsync(c => c.Id == idClient);
        }
        public async Task<Account?> GetAccount(Guid idAccount)
        {
            using var db = new BankContext();
            return await db.Account.FirstOrDefaultAsync(a => a.Id == idAccount);
        }
        public async Task<List<Account>> GetAccounts(Guid idClient)
        {
            using var db = new BankContext();
            return await db.Account.Where(a => a.ClientId == idClient).ToListAsync();
        }

        public async Task AddClient(Client client)
        {
            client.Validation();
            using var db = new BankContext();
            if (!await db.Passport.AnyAsync(p => p.Id == client.PassportId))
                throw new ArgumentNullException("Такого паспорта не было найденно");
            if (await db.Client.AnyAsync(p => p.PassportId == client.PassportId) 
                || await db.Employee.AnyAsync(p => p.PassportId == client.PassportId))
                throw new ArgumentException("Этот паспорт уже используется");
            db.Client.Add(client);
            await db.SaveChangesAsync();
        }
        public async Task UpdateClient(Client client)
        {
            client.Validation();
            using var db = new BankContext();
            if (!await db.Client.AnyAsync(c => c.Id == client.Id))
                throw new ArgumentNullException("Изменяемый клиент не был найден");
            db.Client.Update(client);
            await db.SaveChangesAsync();
        }
        public async Task DeleteClient(Guid idClient)
        {
            using var db = new BankContext();
            var client = await db.Client.FirstOrDefaultAsync(c => c.Id == idClient);
            if (client == null)
                throw new ArgumentNullException("Удаляемый клиент не был найден");
            db.Client.Remove(client);
            await db.SaveChangesAsync();
        }
        public async Task AddAccount(Account account)
        {
            account.Validation();
            using var db = new BankContext();
            if (!await db.Client.AnyAsync(c => c.Id == account.ClientId))
                throw new ArgumentNullException("Такого клиента в реестре банка не существует");
            db.Account.Add(account);
            await db.SaveChangesAsync();
        }
        public async Task ChangeAccountClient(Account account)
        {
            account.Validation();
            using var db = new BankContext();
            if (!await db.Client.AnyAsync(c => c.Id == account.ClientId))
                throw new ArgumentNullException("Такого клиента в реестре банка не существует");
            if (!await db.Account.AnyAsync(a => a.Id == account.Id))
                throw new ArgumentNullException("Такого банковского счета не существует");
            if (!await db.Account.AnyAsync(a => a.ClientId == account.ClientId))
                throw new ArgumentNullException("Такого банковского счета у клиента не существует");
            db.Account.Update(account);
            await db.SaveChangesAsync();
        }
        public async Task DeleteAccountClient(Guid idAccount)
        {
            using var db = new BankContext();
            var account = await db.Account.FirstOrDefaultAsync(a => a.Id == idAccount);
            if (account == null)
                throw new ArgumentNullException("Удаляемый банковский счет не был найден");
            db.Account.Remove(account);
            await db.SaveChangesAsync();
        }
    }
}