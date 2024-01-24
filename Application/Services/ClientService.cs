using Models;
using Services.Storage;
using Models.Validations;
using BankDbConnection;
using Models.Requests;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services
{
    public class ClientService : IClientService
    {
        public ClientService () {}
        public async Task<Client?> GetFirstClient() 
        {
            using var db = new BankContext();
            return await db.Client.FirstOrDefaultAsync();
        }
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
                    var passport = await passportService.Get(c.PassportId);
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
                $"Номер телефона: {client.NumberPhone}\n" + 
                $"В черном списке: {client.InBlackList}\n";
        }
        public async Task<Client?> Get(Guid idClient)
        {
            using var db = new BankContext();
            return await db.Client.FirstOrDefaultAsync(c => c.Id == idClient);
        }

        public async Task Add(Client client)
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
        public async Task Update(Client client)
        {
            client.Validation();
            using var db = new BankContext();
            if (!await db.Client.AnyAsync(c => c.Id == client.Id))
                throw new ArgumentNullException("Изменяемый клиент не был найден");
            db.Client.Update(client);
            await db.SaveChangesAsync();
        }
        public async Task Delete(Guid idClient)
        {
            using var db = new BankContext();
            var client = await db.Client.FirstOrDefaultAsync(c => c.Id == idClient);
            if (client == null)
                throw new ArgumentNullException("Удаляемый клиент не был найден");
            db.Client.Remove(client);
            await db.SaveChangesAsync();
        }
    }
}