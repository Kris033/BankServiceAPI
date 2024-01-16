using Bogus;
using Models;
using Services.Storage;
using Models.Validations;
using BankDbConnection;
using Models.Requests;

namespace Services
{
    public class ClientService
    {
        private readonly IClientStorage _accountsClient;
        private BankContext _bankContext;
        public ClientService() 
        {
            _accountsClient = new ClientStorage();
            _bankContext = new BankContext();
        }
        public Client GetFirstClient() => _accountsClient.Data.FirstOrDefault().Key;

        public KeyValuePair<Client, List<Account>> GetFirstClientWithAccounts() 
            => _accountsClient.Data.FirstOrDefault();
        public List<Client> GetClients(GetFilterRequest? filterRequest = null)
        {
            var clients = _bankContext.Client.AsQueryable();
            if (filterRequest != null)
            {
                var passportService = new PassportService();
                List<Passport> passportList = new List<Passport>();
                clients.ToList().ForEach(c =>
                {
                    var passport = passportService.GetPassport(c.PassportId);
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

        public Client GetYoungestClient() 
            => _bankContext.Client
                .First(e => e
                    .Age == _bankContext.Client
                        .Min(ea => ea.Age));
        public Client GetOldestClient() 
            => _bankContext.Client
                .First(e => e
                    .Age == _bankContext.Client
                        .Max(ea => ea.Age));
        public int GetAverrageAgeClients()
            => _bankContext.Client.Any()
            ? _bankContext.Client.Sum(e => e.Age) / _bankContext.Client.Count()
            : 0;
        public string GetInformation(Client client)
        {
            return $"Имя: {client.Name}\n" +
                $"Возраст: {client.Age}\n" +
                $"Номер телефона: {client.NumberPhone}\n";
        }
        public Client? GetClient(Guid idClient)
            => _bankContext.Client.FirstOrDefault(c => c.Id == idClient);
        public Account? GetAccount(Guid idAccount)
            => _bankContext.Account.FirstOrDefault(a => a.Id == idAccount);
        public List<Account> GetAccounts(Guid idClient) 
            => _bankContext.Account.Where(a => a.ClientId == idClient).ToList();

        public void AddClient(Client client)
        {
            client.Validation();
            using var db = new BankContext();
            if (!db.Passport.Any(p => p.Id == client.PassportId))
                throw new ArgumentNullException("Такого паспорта не было найденно");
            if (db.Client.Any(p => p.PassportId == client.PassportId) 
                || db.Employee.Any(p => p.PassportId == client.PassportId))
                throw new ArgumentException("Этот паспорт уже используется");
            db.Client.Add(client);
            db.SaveChanges();
        }
        public void UpdateClient(Client client)
        {
            client.Validation();
            using var db = new BankContext();
            if (!db.Client.Any(c => c.Id == client.Id))
                throw new ArgumentNullException("Изменяемый клиент не был найден");
            db.Client.Update(client);
            db.SaveChanges();
        }
        public void DeleteClient(Guid idClient)
        {
            using var db = new BankContext();
            var client = db.Client.FirstOrDefault(c => c.Id == idClient);
            if (client == null)
                throw new ArgumentNullException("Удаляемый клиент не был найден");
            db.Client.Remove(client);
            db.SaveChanges();
        }
        public void AddAccount(Account account)
        {
            account.Validation();
            using var db = new BankContext();
            if (!_bankContext.Client.Any(c => c.Id == account.ClientId))
                throw new ArgumentNullException("Такого клиента в реестре банка не существует");
            db.Account.Add(account);
            db.SaveChanges();
        }
        public void ChangeAccountClient(Account account)
        {
            account.Validation();
            using var db = new BankContext();
            if (!db.Client.Any(c => c.Id == account.ClientId))
                throw new ArgumentNullException("Такого клиента в реестре банка не существует");
            if (!db.Account.Any(a => a.Id == account.Id))
                throw new ArgumentNullException("Такого банковского счета не существует");
            if (!db.Account.Any(a => a.ClientId == account.ClientId))
                throw new ArgumentNullException("Такого банковского счета у клиента не существует");
            db.Account.Update(account);
            db.SaveChanges();
        }
        public void DeleteAccountClient(Guid idAccount)
        {
            using var db = new BankContext();
            var account = db.Account.FirstOrDefault(a => a.Id == idAccount);
            if (account == null)
                throw new ArgumentNullException("Удаляемый банковский счет не был найден");
            db.Account.Remove(account);
            db.SaveChanges();
        }
    }
}